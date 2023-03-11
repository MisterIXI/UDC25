using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] float timeToOpen;
    [SerializeField] float openAngle;
    [SerializeField] bool isKeyNeeded;
    [SerializeField] AnimationCurve doorAnimationCurve;
    [SerializeField] int lockID;

    bool hasInteracted;
    public string Data()
    {
        return KeyString();
    }

    string KeyString()
    {
        Key key = PlayerInventory.Instance.Item?.GetComponent<Key>();

        if (isKeyNeeded && key == null)
            return "Door Locked";
        else if(isKeyNeeded && key.KeyID != lockID)
            return "Wrong Key";
        else if (isKeyNeeded && key.KeyID == lockID)
            return "Unlock Door";
        return "Open Door";
    }


    public void Interact()
    {
        if (!hasInteracted)
        {
            if(!isKeyNeeded)
                StartCoroutine(OpenDoor());
            else
            {
                //implement logic for interaction if key is equipped
                CheckKey();
            }
        }
    }

    void CheckKey()
    {
        GameObject playerItem = PlayerInventory.Instance.Item;
        if (playerItem.GetComponent<Key>() && playerItem.GetComponent<Key>().KeyID == lockID)
            StartCoroutine(OpenDoor());
    }

    public void StopAllDoorCoroutines()
    {
        StopAllCoroutines();
    }


    public IEnumerator OpenDoor()
    {

        float duration = 0f;
        float startRotation = transform.rotation.eulerAngles.y; 
        while (duration < timeToOpen)
        {
            float currentAngle = Mathf.Lerp(startRotation, openAngle, doorAnimationCurve.Evaluate(duration/timeToOpen));
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y + currentAngle, transform.rotation.z);
            duration += Time.deltaTime;
            yield return null;
        }
        hasInteracted = true;
    }

    public IEnumerator CloseDoor()
    {
        float duration = 0f;
        float startRotation = transform.rotation.eulerAngles.y;
        while (duration < timeToOpen)
        {
            float currentAngle = Mathf.Lerp(startRotation, 0, doorAnimationCurve.Evaluate(duration / timeToOpen));
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y + currentAngle, transform.rotation.z);
            duration += Time.deltaTime;
            yield return null;
        }
        hasInteracted = false;
    }
}
