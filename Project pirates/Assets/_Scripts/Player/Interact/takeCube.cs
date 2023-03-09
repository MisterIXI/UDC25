using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class takeCube : MonoBehaviour, IInteractable
{
    private PlayerInventory inventory;

    private void Start() {
        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }
    
    public string Data()
    {
        return "Take Cube";
    }

    public void Interact()
    {
        inventory.TakeObject(gameObject);
    }
}