using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 checkpointPos = transform.position;
            CheckpointManager.Instance.SetCheckpoint(checkpointPos);
            Debug.Log("🏁 Checkpoint activado en: " + checkpointPos);
        }
    }
}
