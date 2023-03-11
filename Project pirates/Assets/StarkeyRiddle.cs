using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarkeyRiddle : MonoBehaviour
{
    [SerializeField] GameObject keyObject;

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
                    isKeyObtained = true;
                    keyObject.GetComponent<MeshRenderer>().enabled = true;
                    keyObject.GetComponent<StarKey>().IsKeyObtained(true);
                    keyObject.transform.SetParent(null);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && !isKeyObtained) {
            keyObject.SetActive(false);
            isPlayerInTrigger = false;
        }
    }


    

}

