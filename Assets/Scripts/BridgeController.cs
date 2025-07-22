using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public float moveDuration = 3f;
    public Vector3 targetPosition;
    public Vector3 targetRotation;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isMoving = false;
    private float moveTimer = 0f;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(moveTimer / moveDuration);
            
            transform.position = Vector3.Lerp(originalPosition, targetPosition, progress);
            transform.rotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(targetRotation), progress);
            
            if (progress >= 1f)
            {
                isMoving = false;
            }
        }
    }

    public void ActivateBridge()
    {
        if (!isMoving)
        {
            isMoving = true;
            moveTimer = 0f;
        }
    }
}