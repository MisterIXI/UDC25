using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public string Data()
    {
        Debug.Log("Test");
        return "Test";
    }

    public void Interact()
    {
        Debug.Log("interact with door");
    }
}
