using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testObject : MonoBehaviour, IInteractable
{
    public string Data()
    {
        Debug.Log("Looking at Door.");
        return "open the Door.";
    }

    public void Interact()
    {
        Debug.Log("OpanDaDoor");
    }
}
