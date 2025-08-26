using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public float moveDuration = 3f;
    public Transform targetTransform;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private bool isMoving = false;
    private float moveTimer = 0f;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetRotation = targetTransform.rotation;
        }
        else
        {
            Debug.LogError("targetTransform no está asignado en BridgeController.");
            enabled = false; 
        }
    }

    void Update()
    {
        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(moveTimer / moveDuration);

            transform.position = Vector3.Lerp(originalPosition, targetPosition, progress);
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, progress);

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
