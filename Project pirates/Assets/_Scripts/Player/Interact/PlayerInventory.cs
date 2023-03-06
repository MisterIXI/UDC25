using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    GameObject inventory;
   
    private Camera mainCamera;
    private GameObject item;
    private bool animate = false;


    private void Start() 
    {
        mainCamera = Camera.main;

        inventory = new GameObject("Inventory");
        inventory.tag = "Inventory";
        inventory.transform.SetParent(mainCamera.transform);
        inventory.transform.localPosition = new Vector3(-0.55f, -0.275f, 1);
    }


    private void FixedUpdate() 
    {
        if (item != null && animate == true)
        {   
            item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, Vector3.zero, Time.deltaTime * 4);
            item.transform.rotation = Camera.main.transform.rotation;

            if (item.transform.localPosition == Vector3.zero)
            {
                animate = false;
            }
        }
    }


    public void TakeObject(GameObject newObj)
    {
        item = newObj;

        foreach  (Transform trans in inventory.transform)
        {
            trans.gameObject.GetComponent<Collider>().enabled = true;
            trans.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            // trans.position = item.transform.position;
            trans.SetParent(null);
        }

        newObj.GetComponent<Collider>().enabled = false;
        newObj.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.SetParent(inventory.transform);
        animate = true;
    }
}