using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    private AudioClips _audioClips;

    [SerializeField] float timeToOpen;
    [SerializeField] float openAngle;
    [SerializeField] bool isKeyNeeded;
    [SerializeField] AnimationCurve doorAnimationCurve;
    [SerializeField] int lockID;
    [SerializeField] float angleOffset;

    bool hasInteracted;

    void Start()
    {
        _audioClips = SoundManager.AudioClips;
    }


    public string Data()
    {
        return KeyString();
    }

    string KeyString()
    {
        Key key = PlayerInventory.Instance.Item?.GetComponent<Key>();

        if (isKeyNeeded && key == null)
            return "Door Locked";
        else if (isKeyNeeded && key.KeyID != lockID)
            return "Wrong Key";
        else if (isKeyNeeded && key.KeyID == lockID)
            return "Unlock Door";
        else if (hasInteracted)
            return "Door is Stuck";
        return "Open Door";
    }


    public void Interact()
    {
        if (!hasInteracted)
        {
            if (!isKeyNeeded)
            {
                SoundManager.Instance.PlayAudioOneShotAtPosition(_audioClips.DoorOpenShort, Camera.main.transform.position);
                StartCoroutine(OpenDoor());
            }
            else
            {
                CheckKey();
            }
        }
    }

    void CheckKey()
    {
        Key key = PlayerInventory.Instance.Item?.GetComponent<Key>();
        if (key?.KeyID == lockID)
            SoundManager.Instance.PlayAudioOneShotAtPosition(_audioClips.DoorOpenShort, Camera.main.transform.position);
        StartCoroutine(OpenDoor());
    }

    public void StopAllDoorCoroutines()
    {
        StopAllCoroutines();
    }

    public IEnumerator OpenDoor()
    {
        float duration = 0f;
        float startRotation = transform.localEulerAngles.y;
        while (duration < timeToOpen)
        {
            float currentAngle = Mathf.Lerp(startRotation, openAngle, doorAnimationCurve.Evaluate(duration / timeToOpen));
            transform.localEulerAngles = new Vector3(transform.rotation.x, currentAngle, transform.rotation.z);
            duration += Time.deltaTime;
            yield return null;
        }

        hasInteracted = true;
    }

    public IEnumerator CloseDoor()
    {
        float duration = 0f;
        float startRotation = transform.localEulerAngles.y;
        while (duration < timeToOpen)
        {
            float currentAngle = Mathf.Lerp(startRotation, 0, doorAnimationCurve.Evaluate(duration / timeToOpen));
            transform.localEulerAngles = new Vector3(transform.rotation.x, currentAngle, transform.rotation.z);
            duration += Time.deltaTime;
            yield return null;
        }
        hasInteracted = false;
    }
}
