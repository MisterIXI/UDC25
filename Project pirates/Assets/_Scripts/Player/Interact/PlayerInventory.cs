using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public void TakeObject(GameObject newObj)
    {
        GameObject inventory = GameObject.FindGameObjectWithTag("Inventory");

        foreach  (Transform trans in inventory.transform)
        {
            trans.SetParent(null);
            trans.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            trans.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 2));
        }

        newObj.transform.SetParent(inventory.transform);
        newObj.GetComponent<Rigidbody>().isKinematic = true;

        newObj.transform.localPosition = new Vector3 (0,0,0);
        newObj.transform.localRotation = Quaternion.Euler(0,4,4);
    }
}