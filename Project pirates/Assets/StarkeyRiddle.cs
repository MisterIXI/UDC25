using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarkeyRiddle : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject keyObject;
    private PlayerInventory inventory;

    bool isPlayerInTrigger = false;
    Camera _mainCamera;

    bool isKeyObtained = false;
    private void Start() {
        _mainCamera = Camera.main;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !isKeyObtained) {
            keyObject.SetActive(true);
            isPlayerInTrigger = true;
        }        
    }


    private void Update() {
        if(isPlayerInTrigger && !isKeyObtained){
            RaycastHit hit;
            if(Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward * 100, out hit)){
                if(hit.collider.gameObject.Equals(keyObject)){
                    // data nad interact here
                    isKeyObtained = true;
                }
            }
        }
        Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * 100, Color.red);
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && !isKeyObtained) {
            keyObject.SetActive(false);
            isPlayerInTrigger = false;
        }
    }

    public string Data()
    {
        return "Look at the Star formation";
    }

    public void Interact()
    {
        keyObject.SetActive(true);
        keyObject.GetComponent<MeshRenderer>().enabled = true;
        inventory.TakeObject(keyObject);
    }
}