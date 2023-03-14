using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarKey : MonoBehaviour, IInteractable
{
    private PlayerInventory inventory;

    void Start() {
        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }

    public string Data()
    {
        if (inventory.Item != null && inventory.Item.name == "Spyglass")
        {
            return "Look at the Star formation";
        }
        else
        {
            return "I must have forgotten the Spyglass";
        } 
    }

    public void Interact()
    {
        if (inventory.Item != null && inventory.Item.name == "Spyglass")
        {
            Destroy(GetComponent<SphereCollider>());
            GetComponent<MeshRenderer>().enabled = true;
            PlayerInventory.Instance.TakeObject(gameObject);
        }
    }
}
