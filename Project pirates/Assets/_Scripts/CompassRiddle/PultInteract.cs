using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PultInteract : MonoBehaviour, IInteractable
{
    public GameObject voidDoor;
    public GameObject drawer;

    private GameObject compass;
    private PlayerInventory inventory;
    private PlayerSettings _playerSettings;

    private bool riddleSolved = false;
    private bool firstSequenceFinish = false;
    private Vector3 newPos = new Vector3(0, 0.7f, 0);
    private Quaternion newRot = Quaternion.Euler(6, 0, 0);
    private Vector3 drawerPos = new Vector3(0, 0.5413046f, 0.2431f);

    private Vector3 voidDoorPos = new Vector3(0, 2, 49.94f);


    private void Start() 
    {
        _playerSettings = SettingsManager.PlayerSettings;

        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }


    private void FixedUpdate() 
    {
        if(riddleSolved == true)
        {
            CloseVoidDoor();
            FogOff();               // only works when grammohone room is turned off (the table has fogOn scrip)
            if(PlaceCompass())
            {
                if(OpenDrawer())
                {
                    Destroy(this);
                }
            }
        }
    }


    public string Data()
    {
        if(inventory.Item.name  == "Compass")
        {
            return "Place compass on table.";
        }
        else
        {
            return "Compass is missing.";
        }
    }


    public void Interact()
    {
        if(inventory.Item.name  == "Compass")
        {
            compass = inventory.Item;

            inventory.TakeObject(null);
            compass.transform.SetParent(gameObject.transform);

            compass.GetComponent<Rigidbody>().isKinematic = true;
            compass.GetComponentInChildren<Collider>().enabled = false;

            riddleSolved = true;
        }
    }


    private bool PlaceCompass()  // Animates the compass ontop of table
    {
        if(Vector3.Distance(compass.transform.localPosition, new Vector3(newPos.x, newPos.y + 0.3f, newPos.z)) > 0.05f && !(firstSequenceFinish))
        {
            compass.transform.localPosition = Vector3.Lerp(compass.transform.localPosition, new Vector3(newPos.x, newPos.y + 0.3f, newPos.z), Time.deltaTime);
            compass.transform.localRotation = Quaternion.Lerp(compass.transform.localRotation, newRot, Time.deltaTime * 10);
        }
        else
        {
            firstSequenceFinish = true;
        }

        if(firstSequenceFinish)
        {            
            if(Vector3.Distance(compass.transform.localPosition, newPos) > 0.01f)
            {
                compass.transform.localPosition = Vector3.Lerp(compass.transform.localPosition, newPos, Time.deltaTime);
                compass.transform.localRotation = Quaternion.Lerp(compass.transform.localRotation, newRot, Time.deltaTime * 10);
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }


    private bool OpenDrawer()
    {
        if(Vector3.Distance(drawer.transform.localPosition, drawerPos) > 0.005f)
        {
            drawer.transform.localPosition = Vector3.Lerp(drawer.transform.localPosition, drawerPos, Time.deltaTime);
            return false;
        }
        else
        {
            return true;
        }
    }


    private void FogOff()
    {
        if (RenderSettings.fogDensity != 0)
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0, Time.deltaTime);
        }
        else
        {
            RenderSettings.fog = false;
        }
    }


    private void CloseVoidDoor()
    {
        if(Vector3.Distance(voidDoor.transform.localPosition, voidDoorPos) > 0.01f)
        {
            voidDoor.transform.localPosition = Vector3.Lerp(voidDoor.transform.localPosition, voidDoorPos, Time.deltaTime * 5);
        }
    }
}