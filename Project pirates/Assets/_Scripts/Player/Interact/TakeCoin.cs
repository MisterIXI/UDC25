using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCoin : MonoBehaviour, IInteractable
{
    private PlayerInventory inventory;
    // SEND TO COINMANAGER +1
    // ADD STORE COIN FUNCTION
    // 
    private void Start() {
        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }
    public string Data()
    {
        return "Take Coin";
    }
    public void Interact()
    {
        inventory.TakeObject(gameObject);
    }
}   
