using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarKey : MonoBehaviour, IInteractable
{
    Transform player;

    bool isKeyObtained = false;
    bool isKeyMovingToPlayer = false;
    [SerializeField] float duration = 6f;
    float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isKeyObtained && !isKeyMovingToPlayer){
            StartCoroutine(MoveKeyToPlayer());
            isKeyMovingToPlayer = true;
        }
    }


    IEnumerator MoveKeyToPlayer(){
        float currentTime = 0f;
        Vector3 startPosition = transform.position;
        while(currentTime < duration && Vector3.Distance(transform.position, player.position) > 2f){
            transform.position = Vector3.Lerp(startPosition, player.position, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void IsKeyObtained(bool value){
        isKeyObtained = value;
    }

    public string Data()
    {
        return "Collect Key";
    }

    public void Interact()
    {
        Destroy(GetComponent<SphereCollider>());
        PlayerInventory.Instance.TakeObject(gameObject);
    }
}
