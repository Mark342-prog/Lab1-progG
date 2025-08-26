using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerRespawn respawnController;

    private void Start()
    {
        if (respawnController == null)
            respawnController = GetComponent<PlayerRespawn>();
    }

    public void TakeDamage()
    {
        if (respawnController != null)
        {
            respawnController.Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyTrigger") || other.CompareTag("TurretProjectile"))
        {
            TakeDamage();
        }
    }
}