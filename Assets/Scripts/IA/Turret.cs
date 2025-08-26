using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
    [Header("Turret References")]
    public Transform baseTransform; 
    public Transform headTransform; 
    public Transform firePoint;     
   
    [Header("Turret Settings")]
    public float range = 20f;
    public float fireRate = 1f; 
    public float rotationSpeed = 5f;
    
    [Header("Target Detection")]
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    
    [Header("Visual Effects")]
    public LineRenderer laserLine;
    public float laserDuration = 0.1f;
    
    [Header("Rotation Constraints")]
    public float minElevation = -10f;
    public float maxElevation = 30f;
    
    private Transform player;
    private float fireCooldown = 0f;
    private bool hasLineOfSight = false;

    void Start()
    {
        
        if (baseTransform == null)
        {
            baseTransform = transform.Find("Base2");
            if (baseTransform == null) baseTransform = transform;
        }
        
        if (headTransform == null)
        {
            headTransform = transform.Find("Head");
            if (headTransform == null) headTransform = transform;
        }
    }

    void Update()
    {
        FindNearestPlayer();
        
        if (player == null) return;

    
        RotateBaseTowardsPlayer();


        RotateHeadTowardsPlayer();


        CheckLineOfSight();

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f && hasLineOfSight)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    void RotateBaseTowardsPlayer()
    {
        if (player == null) return;


        Vector3 direction = player.position - baseTransform.position;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            baseTransform.rotation = Quaternion.Slerp(baseTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void RotateHeadTowardsPlayer()
    {
        if (player == null || headTransform == null) return;


        Vector3 direction = player.position - headTransform.position;
        
        float distance = direction.magnitude;
        float angle = Mathf.Asin(direction.y / distance) * Mathf.Rad2Deg;
        
        angle = Mathf.Clamp(angle, minElevation, maxElevation);
        

        Vector3 baseEuler = baseTransform.eulerAngles;
        headTransform.localEulerAngles = new Vector3(angle, 0, 0);

        headTransform.eulerAngles = new Vector3(headTransform.eulerAngles.x, baseEuler.y, headTransform.eulerAngles.z);
    }

    void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject playerObj in players)
        {
            float distance = Vector3.Distance(transform.position, playerObj.transform.position);
            if (distance < closestDistance && distance <= range)
            {
                closestDistance = distance;
                closestPlayer = playerObj.transform;
            }
        }
        
        player = closestPlayer;
    }

    void CheckLineOfSight()
    {
        hasLineOfSight = false;
        if (player == null || firePoint == null) return;

        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        RaycastHit hit;
        
        if (Physics.Raycast(firePoint.position, directionToPlayer, out hit, range, playerLayer | obstacleLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hasLineOfSight = true;
            }
        }
    }

    void Shoot()
    {
        if (player == null || firePoint == null) return;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Impacto en jugador: " + hit.collider.name);
                
                PlayerRespawn playerRespawn = hit.collider.GetComponent<PlayerRespawn>();
                if (playerRespawn != null)
                {
                    playerRespawn.Respawn();
                }
                else
                {
                    Debug.LogWarning("El jugador no tiene componente PlayerRespawn");
                }
            }
        }

        if (laserLine != null)
        {
            Vector3 endPoint = hasLineOfSight && player != null ? 
                player.position : 
                firePoint.position + firePoint.forward * range;
                
            laserLine.SetPosition(0, firePoint.position);
            laserLine.SetPosition(1, endPoint);
            StartCoroutine(LaserEffect());
        }
    }

    IEnumerator LaserEffect()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        
        if (firePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(firePoint.position, firePoint.forward * range);
        }
    }
}