using UnityEngine;
using System.Collections.Generic;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;
    
    public List<CollectibleItem> allCollectibles = new List<CollectibleItem>();
    public GameObject bridgeSwitch; 
    public BridgeController bridgeController; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterCollectible(CollectibleItem collectible)
    {
        if (!allCollectibles.Contains(collectible))
            allCollectibles.Add(collectible);
    }

    public void CheckAllCollected()
    {
        bool allCollected = true;
        
        foreach (CollectibleItem item in allCollectibles)
        {
            if (item != null && item.gameObject.activeSelf)
            {
                allCollected = false;
                break;
            }
        }

        if (allCollected && bridgeSwitch != null)
        {
            bridgeSwitch.SetActive(true);
            Debug.Log("¡Todos los objetos recolectados! El interruptor está activo.");
        }
    }
}