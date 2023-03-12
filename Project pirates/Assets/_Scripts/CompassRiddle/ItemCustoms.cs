using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCustoms : MonoBehaviour
{
    public GameObject compassRoomDeco;
    public GameObject spyglassRoomDeco;
    public GameObject voidRoom;

    private PlayerInventory inventory;


    private void Start() 
    {
        inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
    }


    private void OnTriggerEnter(Collider other) 
    {
        if (!(other.gameObject.CompareTag("Player")) && other.gameObject != inventory.Item && !(other.gameObject.CompareTag("Coin")))
        {
            if(other.transform.IsChildOf(compassRoomDeco.transform))
            {
                Debug.Log("Item parented to void room.");
                other.gameObject.transform.SetParent(voidRoom.transform);
            }
            else if(other.transform.IsChildOf(voidRoom.transform) && compassRoomDeco.activeSelf)
            {
                Debug.Log("Item parented to compass room.");
                other.gameObject.transform.SetParent(compassRoomDeco.transform);
            }
            else if(other.transform.IsChildOf(voidRoom.transform) && spyglassRoomDeco.activeSelf)
            {
                Debug.Log("Item parented to spyglass room.");
                other.gameObject.transform.SetParent(spyglassRoomDeco.transform);
            }
        }
    }
}
