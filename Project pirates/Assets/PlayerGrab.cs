using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGrab : MonoBehaviour
{
    private void SubscribeToInput()
    {
        InputManager.OnInteract += OnInteract;
    }

    void OnInteract(InputAction.CallbackContext context)
    {

    }
}
