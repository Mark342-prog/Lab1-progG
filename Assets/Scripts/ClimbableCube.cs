using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ClimbableCube : MonoBehaviour
{
    [Header("Propiedades de Escalada")]
    public float climbForce = 8f;
    public float playerCheckRadius = 0.5f;
    public LayerMask playerLayer;
    
    private void FixedUpdate()
    {
        Collider[] players = Physics.OverlapBox(
            transform.position + Vector3.up * 0.5f,
            new Vector3(0.4f, 0.1f, 0.4f),
            Quaternion.identity,
            playerLayer
        );
        
        foreach (Collider player in players)
        {
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
    
                playerRb.AddForce(Vector3.up * climbForce, ForceMode.Acceleration);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            transform.position + Vector3.up * 0.5f,
            new Vector3(0.8f, 0.2f, 0.8f)
        );
    }
}