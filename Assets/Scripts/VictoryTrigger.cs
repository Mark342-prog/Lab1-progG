using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject winScreenUI;
    private AudioSource victoryAudio;

    private void Start()
    {
        victoryAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (victoryAudio != null)
            {
                victoryAudio.Play();
            }

            Time.timeScale = 0f;
            winScreenUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
    }
}
