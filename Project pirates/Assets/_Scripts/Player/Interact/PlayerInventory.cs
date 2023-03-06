using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    GameObject inventory;
    public Vector3 destItemRotation;
    public float rubberbandForce;
    public float rotationSpeed;
    private Camera mainCamera;
    private GameObject item;

    private void Start() 
    {
        mainCamera = Camera.main;
        inventory = new GameObject("Inventory");
        inventory.tag = "Inventory";
        inventory.transform.SetParent(mainCamera.transform);
        inventory.transform.localPosition = new Vector3(-0.55f, -0.275f, 1); //change this for size and position of items
    }


    private void FixedUpdate() 
    {
        if (item != null)
        {
            MoveObject();
            Vector3 startRot = item.transform.localEulerAngles;   
                    Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(startRot.x, destItemRotation.x, Time.fixedDeltaTime * rotationSpeed),
            Mathf.LerpAngle(startRot.y, destItemRotation.y, Time.fixedDeltaTime * rotationSpeed),
            Mathf.LerpAngle(startRot.z, destItemRotation.z, Time.fixedDeltaTime * rotationSpeed));
            item.transform.localEulerAngles = currentAngle;
        }
    }

    private void MoveObject()
    {
        if (Vector3.Distance(item.transform.position, inventory.transform.position) > 0.1f)
        { 
            Vector3 moveDirection = (inventory.transform.position - item.transform.position);
            item.GetComponent<Rigidbody>().AddForce(moveDirection * rubberbandForce);
        }

        if(Vector3.Distance(item.transform.position, inventory.transform.position) > 5f)
        {
            item.transform.position = inventory.transform.position;
        }
    }

    public void TakeObject(GameObject newObj)
    {
        item = newObj;

        foreach  (Transform trans in inventory.transform)
        {
            trans.gameObject.GetComponent<Collider>().enabled = true;
            // trans.position = item.transform.position;
            trans.SetParent(null);
        }
        newObj.GetComponent<Rigidbody>().useGravity = false;
        newObj.GetComponent<Rigidbody>().drag = 10;
        newObj.GetComponent<Rigidbody>().angularDrag = 3;
        item.transform.SetParent(inventory.transform);
    }

}