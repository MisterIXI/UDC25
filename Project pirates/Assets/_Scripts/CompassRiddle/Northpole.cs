using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Northpole : MonoBehaviour
{
    public GameObject voidDoor;
    public GameObject CompassDecoration;
    public GameObject SpyglassDecoration;
    public Vector3[] spawnPoints;

    private PlayerInventory inventory;

    private int posCount = 0;

    private Vector3 voidDoorClosed = new Vector3(0, 2, 49.94f);
    private Vector3 voidDoorOpend;


    void Start()
    {
        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
        voidDoorOpend = voidDoor.transform.localPosition;

        transform.localPosition = spawnPoints[0];
    }


    void FixedUpdate()
    {
        UpdateSpawn();

        if(posCount - spawnPoints.Length >= -1)
        {
            OpenVoidDoor();
            gameObject.transform.localScale = new Vector3(0.1f,0.2f,0.1f);
        }
        else if(posCount == 1)
        {
            CloseVoidDoor();
            SwapRoomToCompassDeco();
        }
    }


    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(inventory.Item.name  == "Compass")
            {
                posCount += 1;
            }
        }
    }


    private void UpdateSpawn()
    {
        if(posCount <= spawnPoints.Length -1)
        {
            transform.localPosition = spawnPoints[posCount];
        }
    }


    private void CloseVoidDoor()
    {
        if(Vector3.Distance(voidDoor.transform.localPosition, voidDoorClosed) > 0.01f)
        {
            voidDoor.transform.localPosition = Vector3.Lerp(voidDoor.transform.localPosition, voidDoorClosed, Time.deltaTime * 5);
        }
    }


    private void OpenVoidDoor()
    {
        if(Vector3.Distance(voidDoor.transform.localPosition, voidDoorOpend) > 0.01f)
        {
            voidDoor.transform.localPosition = Vector3.Lerp(voidDoor.transform.localPosition, voidDoorOpend, Time.deltaTime * 5);
        }
    }

    private void SwapRoomToCompassDeco()
    {
        CompassDecoration.SetActive(false);
        SpyglassDecoration.SetActive(true);
    }
}