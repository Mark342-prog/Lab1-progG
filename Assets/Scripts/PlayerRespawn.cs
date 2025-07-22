using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private void Start()
    {

        CheckpointManager.Instance.SetCheckpoint(transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(" R presionado, intentando respawn...");
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyTrigger"))
        {
            Debug.Log(" Tocó trampa. Respawn...");
            Respawn();
        }
    }

void Respawn()
{
    Vector3 checkpoint = CheckpointManager.Instance.GetCheckpoint();

    CharacterController cc = GetComponent<CharacterController>();

    if (cc != null)
    {
        cc.enabled = false; 
        transform.position = checkpoint;
        cc.enabled = true; 
    }
    else
    {
        transform.position = checkpoint;
    }

    Debug.Log(" Respawn a: " + checkpoint);
}
}
