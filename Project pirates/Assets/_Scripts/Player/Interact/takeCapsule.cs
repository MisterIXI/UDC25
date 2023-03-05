using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeCapsule : MonoBehaviour, IInteractable
{
    public GameObject _player;
    public string Data()
    {
        return "Take Capsule";
    }

    public void Interact()
    {
        _player.GetComponent<PlayerInventory>().TakeObject(gameObject);
    }
}