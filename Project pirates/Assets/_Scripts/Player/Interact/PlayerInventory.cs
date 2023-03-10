using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerInventory : MonoBehaviour
{
    GameObject inventory;
    private Camera mainCamera;
    public GameObject Item { get; private set; }
    private PlayerSettings _playerSettings;
    public static PlayerInventory Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    private void Start()
    {
        _playerSettings = SettingsManager.PlayerSettings;
        mainCamera = Camera.main;
        inventory = new GameObject("Inventory");
        inventory.tag = "Inventory";
        inventory.transform.SetParent(mainCamera.transform);
        inventory.transform.localPosition = _playerSettings.InvItemPosition;
        inventory.transform.localRotation = Quaternion.identity;
    }


    private void FixedUpdate()
    {
        if (Item != null)
        {
            MoveObject();
            Vector3 startRot = Item.transform.localEulerAngles;
            Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(startRot.x, _playerSettings.InvItemRotation.x, Time.fixedDeltaTime * _playerSettings.InvRotationSpeed),
            Mathf.LerpAngle(startRot.y, _playerSettings.InvItemRotation.y, Time.fixedDeltaTime * _playerSettings.InvRotationSpeed),
            Mathf.LerpAngle(startRot.z, _playerSettings.InvItemRotation.z, Time.fixedDeltaTime * _playerSettings.InvRotationSpeed));
            Item.transform.localEulerAngles = currentAngle;
        }
    }

    private void MoveObject()
    {
        if (Vector3.Distance(Item.transform.position, inventory.transform.position) > 0.01f)
        {
            Vector3 moveDirection = (inventory.transform.position - Item.transform.position);
            Item.GetComponent<Rigidbody>().AddForce(moveDirection * _playerSettings.InvRubberbandForce);
        }

        if (Vector3.Distance(Item.transform.position, inventory.transform.position) > 5f)
        {
            Item.transform.position = inventory.transform.position;
        }
    }

    public void TakeObject(GameObject newObj)
    {
        Item = newObj;
        foreach (Transform trans in inventory.transform)
        {
            Rigidbody transRB = trans.GetComponent<Rigidbody>();
            UpdateRigidbody(transRB, false);
            transRB.AddForce(mainCamera.transform.forward * _playerSettings.InteractThrowMagnitude, ForceMode.Impulse);
            // trans.position = item.transform.position;
            trans.SetParent(null);
        }
        if (Item != null)
        {
            UpdateRigidbody(Item.GetComponent<Rigidbody>(), true);
            Item.transform.SetParent(inventory.transform);
            Item.GetComponentsInChildren<Collider>().Any(x => x.enabled = false);
        }
    }

    private void UpdateRigidbody(Rigidbody rb, bool value)
    {
        rb.useGravity = !value;
        rb.drag = value ? 10 : 1;
        rb.angularDrag = value ? 3 : 0.05f;
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}