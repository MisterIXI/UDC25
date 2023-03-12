using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCoin : MonoBehaviour, IInteractable
{
 
    public enum TakeCoinType
    {
        Bronze=0,
        Silver=1,
        Gold=2
    }
    public TakeCoinType coinType;
    private PlayerInventory inventory;
    private bool isUsed = false;
    // SEND TO COINMANAGER +1
    // ADD STORE COIN FUNCTION
    // 
    private void Start() {
        
        // inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }
    public string Data()
    {
        return "Take Breadcrumb";
    }
    public void Interact()
    {
        if(!isUsed)
        {
            isUsed = true;
            //inventory.TakeObject(gameObject);
            CoinManager.Instance.AddCoin(gameObject);
            //BOOLEAN IS USED SO NO DUPE CHANCE
            
        }
    }
    
}   
