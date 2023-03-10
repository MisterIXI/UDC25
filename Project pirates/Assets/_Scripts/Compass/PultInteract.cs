using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PultInteract : MonoBehaviour, IInteractable
{
    private GameObject compass;
    private GameObject drawer;
    private PlayerInventory inventory;

    private bool animate = false;
    private Vector3 newPos = new Vector3(0, 0.7f, 0);
    private Quaternion newRot = Quaternion.Euler(-6, 0, 0);
    private Vector3 drawerPos = new Vector3(0, 0.5413046f, 0.2431f);


    private void Start() 
    {
        // inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
        // drawer = GetComponentInChildren<GameObject>();
    }


    private void FixedUpdate() 
    {
        // if(animate == true)
        // {
        //     compass.transform.localPosition = Vector3.Lerp(compass.transform.localPosition, newPos, Time.deltaTime);
        //     compass.transform.localRotation = Quaternion.Lerp(compass.transform.localRotation, newRot, Time.deltaTime);
            
        //     if (compass.transform.localPosition == newPos && compass.transform.localRotation == newRot)
        //     {
        //         drawer.transform.localPosition = Vector3.Lerp(drawer.transform.localPosition, drawerPos, Time.deltaTime);

        //         if(drawer.transform.localPosition == drawerPos)
        //         {
        //             GetComponent<PultInteract>().enabled = false;
        //         }
        //     }
        // }

    }


    public string Data()
    {
        return "Place Compass on Table.";
    }


    public void Interact()
    {
        // GameObject invent = GameObject.FindGameObjectWithTag("Inventory");
        // compass = invent.GetComponentInChildren<GameObject>();

        // compass.transform.SetParent(gameObject.transform);
        // inventory.TakeObject(null);

        // // compass.GetComponent<Rigidbody>().isKinematic = true;
        // // compass.GetComponent<CompassInteract>().enabled = false;

        // animate = true;
    }
}