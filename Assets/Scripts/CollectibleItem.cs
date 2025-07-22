using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public GameObject effectPrefab;
    public AudioClip collectSound;

    void Start()
    {

        CollectibleManager.Instance?.RegisterCollectible(this);
    }

    public void Collect()
    {
        
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }

        CollectibleManager.Instance?.CheckAllCollected();

        Destroy(gameObject, 0.1f);
    }
}