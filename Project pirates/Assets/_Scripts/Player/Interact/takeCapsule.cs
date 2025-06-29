using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeCapsule : MonoBehaviour, IInteractable
{
    private PlayerInventory inventory;

    private void Start() {
        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }

    public string Data()
    {
        return "Take Capsule";
    }

    public void Interact()
    {
        inventory.TakeObject(gameObject);
    }
}