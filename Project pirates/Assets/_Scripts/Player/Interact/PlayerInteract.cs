using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] public float interactRange = 4f;

    private Camera mainCamera;
    private IInteractable currentInteractableObject;

    // private GameObject ui;
    // private TMP_Text text;

    private void SubscribeToInput(){InputManager.OnInteract += OnInteract;}
    private void UnsubscribeToInput(){InputManager.OnInteract -= OnInteract;}

    void Start() 
    {
        mainCamera = Camera.main;

        // ui = GameObject.FindGameObjectWithTag("UI_Interact");
        // text = ui.GetComponentInChildren<TMP_Text>();

        SubscribeToInput();
    }


    void FixedUpdate()
    {
        Vector3 fwd = mainCamera.transform.forward;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            InteractWithObject(hit.collider.gameObject);
        }
        else
        {
            // ui.SetActive(false);
            currentInteractableObject = null;
        }

        Debug.DrawRay(mainCamera.transform.position, fwd * interactRange, Color.red);
    }


    void InteractWithObject(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out IInteractable interactableObject))
        {
            currentInteractableObject = interactableObject;

            // ui.SetActive(true);
            // text.text = "Press " + "LMB" + " to " + currentInteractableObject.Data();
            currentInteractableObject.Data();
        }
        else
        {
            currentInteractableObject = null;
        }
    }


    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && currentInteractableObject != null)
        {
            currentInteractableObject.Interact();
        }
    }

    private void OnDestroy() {
        UnsubscribeToInput();
    }
}