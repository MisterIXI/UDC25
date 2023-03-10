using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    GameObject inventory;
    private Camera mainCamera;
    private GameObject item;
    private PlayerSettings _playerSettings;
    private void Start()
    {
        _playerSettings = SettingsManager.PlayerSettings;
        mainCamera = Camera.main;
        inventory = new GameObject("Inventory");
        inventory.tag = "Inventory";
        inventory.transform.SetParent(mainCamera.transform);
        inventory.transform.localPosition = _playerSettings.InvItemPosition;
    }


    private void FixedUpdate()
    {
        if (item != null)
        {
            MoveObject();
            Vector3 startRot = item.transform.localEulerAngles;
            Vector3 currentAngle = new Vector3(
    Mathf.LerpAngle(startRot.x, _playerSettings.InvItemRotation.x, Time.fixedDeltaTime * _playerSettings.InvRotationSpeed),
    Mathf.LerpAngle(startRot.y, _playerSettings.InvItemRotation.y, Time.fixedDeltaTime * _playerSettings.InvRotationSpeed),
    Mathf.LerpAngle(startRot.z, _playerSettings.InvItemRotation.z, Time.fixedDeltaTime * _playerSettings.InvRotationSpeed));
            item.transform.localEulerAngles = currentAngle;
        }
    }

    private void MoveObject()
    {
        if (Vector3.Distance(item.transform.position, inventory.transform.position) > 0.01f)
        {
            Vector3 moveDirection = (inventory.transform.position - item.transform.position);
            item.GetComponent<Rigidbody>().AddForce(moveDirection * _playerSettings.InvRubberbandForce);
        }

        if (Vector3.Distance(item.transform.position, inventory.transform.position) > 5f)
        {
            item.transform.position = inventory.transform.position;
        }
    }

    public void TakeObject(GameObject newObj)
    {
        item = newObj;
        foreach (Transform trans in inventory.transform)
        {
            trans.gameObject.GetComponent<Collider>().enabled = true;
            Rigidbody transRB = trans.GetComponent<Rigidbody>();
            UpdateRigidbody(transRB, false);
            transRB.AddForce(mainCamera.transform.forward * _playerSettings.InteractThrowMagnitude, ForceMode.Impulse);
            // trans.position = item.transform.position;
            trans.SetParent(null);
        }
        UpdateRigidbody(item.GetComponent<Rigidbody>(), true);
        item.transform.SetParent(inventory.transform);
    }

    private void UpdateRigidbody(Rigidbody rb, bool value)
    {
        rb.useGravity = !value;
        rb.drag = value ? 10 : 1;
        rb.angularDrag = value ? 3 : 0.05f;
    }

}