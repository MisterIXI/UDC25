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
    }


    private void FixedUpdate() 
    {
        if (item != null)
        {   
            Vector3 targetPos = mainCamera.ScreenToWorldPoint(new Vector3(0.3f,0.3f,1));

            item.transform.position = Vector3.Lerp(item.transform.position, targetPos, Time.deltaTime * 10);

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