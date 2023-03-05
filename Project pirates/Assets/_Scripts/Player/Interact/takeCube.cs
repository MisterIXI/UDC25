using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeCube : MonoBehaviour, IInteractable
{
    public GameObject _player;
    public string Data()
    {
        return "Take Cube";
    }

    public void Interact()
    {
        _player.GetComponent<PlayerInventory>().TakeObject(gameObject);
    }
}

