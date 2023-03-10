using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassInteract : MonoBehaviour, IInteractable
{
    private PlayerInventory inventory;

    private void Start() {
        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }
    
    public string Data()
    {
        return "Pickup Compass";
    }

    public void Interact()
    {
        inventory.TakeObject(gameObject);
    }
}