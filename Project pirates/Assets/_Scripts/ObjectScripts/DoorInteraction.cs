using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] float timeToOpen;
    [SerializeField] float openAngle;
    [SerializeField] bool isKeyNeeded;
    [SerializeField] AnimationCurve doorAnimationCurve;
    bool hasInteracted;
    public string Data()
    {
        return null;
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
            }
        }
    }

    IEnumerator OpenDoor()
    {

        float duration = 0f;
        float startRotation = transform.parent.rotation.eulerAngles.y;
        while (duration < timeToOpen)
        {
            float currentAngle = Mathf.Lerp(startRotation, openAngle, doorAnimationCurve.Evaluate(duration/timeToOpen));
            transform.parent.eulerAngles = new Vector3(transform.parent.rotation.x, transform.parent.rotation.y + currentAngle, transform.parent.rotation.z);
            duration += Time.deltaTime;
            yield return null;
        }
        hasInteracted = true;
    }
}
