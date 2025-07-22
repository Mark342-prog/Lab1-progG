using UnityEngine;
using UnityEngine.UI;

public class PlayerCollector : MonoBehaviour
{
    [Header("Configuración")]
    public KeyCode interactionKey = KeyCode.E;
    public float interactionRange = 2f;

    [Header("UI")]
    public Text itemCounterText;
    public GameObject interactionPrompt;

    [Header("Interacción con Cubo")]
    public float grabDistance = 2f;
    public float throwForce = 5f;
    public Transform holdPosition;
    public LayerMask movableLayer;

    private int itemsCollected = 0;
    private CollectibleItem currentItem;
    private GameObject heldCube;
    private Rigidbody heldCubeRb;
    private BridgeSwitch currentBridgeSwitch;

    void Update()
    {
        CheckForInteractables();
        UpdateUI();
        
        HandleInteractionInput();
        
        if (heldCube != null)
        {
            heldCube.transform.position = holdPosition.position;
            heldCube.transform.rotation = holdPosition.rotation;
        }
    }

    void CheckForInteractables()
{
    RaycastHit hit;
    bool hitDetected = Physics.Raycast(
        transform.position,
        transform.forward,
        out hit,
        interactionRange
    );

    if (hitDetected)
    {
        if (hit.collider.CompareTag("Collectible"))
        {
            currentItem = hit.collider.GetComponent<CollectibleItem>();
            ShowInteractionPrompt("Recoger [E]");
        }
        else if (IsMovableObject(hit.collider.gameObject))
        {
            ShowInteractionPrompt(heldCube != null ? "Soltar [E]" : "Agarrar [E]");
        }
        else if (hit.collider.CompareTag("BridgeSwitch"))
        {
            currentBridgeSwitch = hit.collider.GetComponent<BridgeSwitch>();
            ShowInteractionPrompt("Activar puente [E]");
        }
        else
        {
            HideInteractionPrompt();
        }
    }
    else
    {
        HideInteractionPrompt();
    }
}

void HandleInteractionInput()
{
    if (Input.GetKeyDown(interactionKey))
    {
        if (currentItem != null)
        {
            CollectItem();
        }
        else if (heldCube == null)
        {
            TryGrabCube();
        }
        else
        {
            ReleaseCube();
        }
    
        if (currentBridgeSwitch != null)
        {
            currentBridgeSwitch.ActivateBridge();
            currentBridgeSwitch = null;
        }
    }
}
    bool IsMovableObject(GameObject obj)
    {
        return obj != null && 
               movableLayer == (movableLayer | (1 << obj.layer)) &&
               obj.GetComponent<Rigidbody>() != null;
    }

    void ShowInteractionPrompt(string message)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
            Text promptText = interactionPrompt.GetComponentInChildren<Text>();
            if (promptText != null) promptText.text = message;
        }
    }

    void HideInteractionPrompt()
    {
        if (interactionPrompt != null) 
            interactionPrompt.SetActive(false);
        currentItem = null;
    }

    void CollectItem()
    {
        itemsCollected++;
        currentItem.Collect();
        currentItem = null;
        HideInteractionPrompt();
    }

void TryGrabCube()
{
    RaycastHit hit;
    int layerMask = movableLayer | (1 << LayerMask.NameToLayer("Default"));
    
    if (Physics.Raycast(
        transform.position,
        transform.forward,
        out hit,
        grabDistance,
        layerMask
    ))
    {
        if (IsMovableObject(hit.collider.gameObject))
        {
            heldCube = hit.collider.gameObject;
            heldCubeRb = heldCube.GetComponent<Rigidbody>();

            heldCubeRb.linearVelocity = Vector3.zero;
            heldCubeRb.angularVelocity = Vector3.zero;
            
            heldCubeRb.isKinematic = true;
            heldCubeRb.useGravity = false;
            
            Physics.IgnoreCollision(
                GetComponent<Collider>(),
                heldCube.GetComponent<Collider>(),
                true
            );

            heldCube.transform.SetParent(null);
        }
    }
}

    void ReleaseCube()
    {
        if (heldCube == null) return;

        heldCubeRb.isKinematic = false;
        heldCubeRb.useGravity = true;

        Physics.IgnoreCollision(
            GetComponent<Collider>(),
            heldCube.GetComponent<Collider>(),
            false
        );

        heldCubeRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        
        heldCube = null;
        heldCubeRb = null;
    }

    void UpdateUI()
    {
        if (itemCounterText != null)
        {
            itemCounterText.text = $"Objetos: {itemsCollected}";
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * interactionRange);
    }
}