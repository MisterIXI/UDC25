using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] public float interactRange = 4f;

    private Camera mainCamera;
    private IInteractable currentInteractableObject;

    [SerializeField] GameObject pickedObject;
    [SerializeField] float moveForce;
    [SerializeField] Transform parentHold;
    Ray _ray;
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

    RaycastHit hit;
    void FixedUpdate()
    {
        Vector3 fwd = mainCamera.transform.forward;
        _ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(_ray, out hit, interactRange))
        {
            InteractWithObject(hit.collider.gameObject);
        }
        else
        {
            // ui.SetActive(false);
            currentInteractableObject = null;
        }

        if(pickedObject != null)
        {
            parentHold.position = transform.position + (mainCamera.transform.forward * 2f);
            MoveObject();
        }

        Debug.DrawRay(mainCamera.transform.position, fwd * interactRange, Color.red);
    }

    private void MoveObject()
    {
        if(Vector3.Distance(pickedObject.transform.position, parentHold.position) > 0.1f)
        {
            Vector3 moveDirection = (parentHold.position - pickedObject.transform.position);
            pickedObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
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
        if (context.performed)
        {
            if (pickedObject == null)
                CheckForPickable();
            else
                DropObject();
        }
    }

    private void DropObject()
    {
        Rigidbody objRig = pickedObject.GetComponent<Rigidbody>();
        objRig.AddForce(_ray.direction * 10f, ForceMode.Impulse);
        objRig.useGravity = true;
        objRig.drag = 1;
        objRig.transform.parent = null;
        pickedObject = null;
    }

    private void CheckForPickable()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.tag.Equals("Pickable"))
            {
                PickupObject(hit.collider.gameObject);
            }
        }
    }

    private void OnDestroy() {
        UnsubscribeToInput();
    }

    void PickupObject(GameObject gameObject)
    {
        pickedObject = gameObject;
        Rigidbody objRig = pickedObject.GetComponent<Rigidbody>();
        objRig.useGravity = false;
        objRig.drag = 10;
        objRig.transform.parent = parentHold;
        pickedObject.transform.position = parentHold.position;
    }
}