using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private GameObject inventory;
    private GameObject item;
    private Camera mainCamera;


    private void Start() 
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        mainCamera = Camera.main;

        foreach  (Transform trans in inventory.transform)   // clears inventory at start
        {
            trans.position = inventory.transform.position;
            trans.SetParent(null);
        }        

        // inventory.transform.SetParent(mainCamera.transform);
        
    }


    private void FixedUpdate() 
    {
        if (item != null)
        {   
            item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, Vector3.zero, Time.deltaTime * 4);
            item.transform.rotation = Camera.main.transform.rotation;
        }
    }


    public void TakeObject(GameObject newObj)
    {
        item = newObj;

        foreach  (Transform trans in inventory.transform)
        {
            trans.gameObject.GetComponent<Collider>().enabled = true;
            trans.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            trans.position = item.transform.position;
            trans.SetParent(null);
        }

        newObj.GetComponent<Collider>().enabled = false;
        newObj.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.SetParent(inventory.transform);
    }
}