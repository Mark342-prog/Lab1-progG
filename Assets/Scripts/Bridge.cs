using UnityEngine;

public class BridgeSwitch : MonoBehaviour
{
    public GameObject interactionPrompt;
    public BridgeController bridgeController;

    private bool canInteract = false;

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            ActivateBridge();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }

    public void ActivateBridge()
    {
        if (bridgeController != null)
        {
            bridgeController.ActivateBridge();
            Debug.Log("¡Puente activado!");
            
            gameObject.SetActive(false);
        }
    }
}