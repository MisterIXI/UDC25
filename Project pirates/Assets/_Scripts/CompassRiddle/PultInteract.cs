using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PultInteract : MonoBehaviour, IInteractable
{
    private AudioClips _audioClips;

    public GameObject voidRoom;
    public GameObject voidDoor;
    public GameObject drawer;


    private GameObject compass;
    private GameObject particles;
    private PlayerInventory inventory;
    private PlayerSettings _playerSettings;

    private bool riddleSolved = false;
    private bool firstSequenceFinish = false;
    private Vector3 newPos = new Vector3(0, 0.695f, 0);
    private Quaternion newRot = Quaternion.Euler(6, 0, 0);
    private Vector3 drawerPos = new Vector3(0, 0.5413046f, 0.2431f);

    private Vector3 grammophoneVoidDoorPos = new Vector3(0, 0, 7.367f);


    private void Start()
    {
        _audioClips = SoundManager.AudioClips;

        _playerSettings = SettingsManager.PlayerSettings;

        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }


    private void FixedUpdate()
    {
        if (riddleSolved == true)
        {
            CloseVoidDoor();
            FogOff();               // only works when grammohone room is turned off (the table has fogOn scrip)
            if (PlaceCompass())
            {
                DisableVoidRoom();
                if (OpenDrawer())
                {
                    SoundManager.Instance.PlayAudioOneShotAtPosition(_audioClips.SomethingHappend, Camera.main.transform.position);

                    particles.SetActive(false);
                    FlagManager.SetFlag("CompassPuzzleSolved", true);
                    Destroy(this);
                }
            }
        }
    }


    public string Data()
    {
        if ((inventory.Item != null && inventory.Item.name == "Compass") || riddleSolved)
        {
            return "Place Compass on Table";
        }
        else
        {
            return "Compass is Missing";
        }
    }


    public void Interact()
    {
        if (inventory.Item != null && inventory.Item.name == "Compass")
        {
            compass = inventory.Item;
            particles = compass.transform.Find("SocketInEffect").gameObject;

            inventory.TakeObject(null);
            compass.transform.SetParent(gameObject.transform);

            compass.GetComponent<Rigidbody>().isKinematic = true;
            compass.GetComponentInChildren<Collider>().enabled = false;

            riddleSolved = true;
        }
    }


    private bool PlaceCompass()  // Animates the compass ontop of table
    {
        if (Vector3.Distance(compass.transform.localPosition, new Vector3(newPos.x, newPos.y + 0.3f, newPos.z)) > 0.05f && !(firstSequenceFinish))
        {
            compass.transform.localPosition = Vector3.Lerp(compass.transform.localPosition, new Vector3(newPos.x, newPos.y + 0.3f, newPos.z), Time.deltaTime);
            compass.transform.localRotation = Quaternion.Lerp(compass.transform.localRotation, newRot, Time.deltaTime * 10);
        }
        else
        {
            firstSequenceFinish = true;
        }

        if (firstSequenceFinish)
        {
            if (Vector3.Distance(compass.transform.localPosition, newPos) > 0.01f)
            {
                particles.SetActive(true);
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
        if (Vector3.Distance(drawer.transform.localPosition, drawerPos) > 0.005f)
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
        if (Vector3.Distance(voidDoor.transform.localPosition, grammophoneVoidDoorPos) > 0.01f)
        {
            voidDoor.transform.localPosition = Vector3.Lerp(voidDoor.transform.localPosition, grammophoneVoidDoorPos, Time.deltaTime * 5);
        }
    }

    private void DisableVoidRoom()
    {
        if (VolumeManager.Sun.intensity != 0)
        {
            VolumeManager.SetSunIntensity(Mathf.Lerp(VolumeManager.Sun.intensity, 0, Time.deltaTime));
        }
        else
        {
            voidRoom.SetActive(false);
        }
    }
}