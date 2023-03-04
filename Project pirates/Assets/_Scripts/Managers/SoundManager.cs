using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] int amountOfAudioEntities;
    [SerializeField] GameObject audioEntity;
    List<GameObject> audioEntities;

    public static SoundManager Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        audioEntities = new List<GameObject>();
        for (int i = 0; i < amountOfAudioEntities; i++)
        {
            GameObject spawnedEntity = Instantiate(audioEntity, transform);
            spawnedEntity.SetActive(false);
            audioEntities.Add(spawnedEntity);
        }
    }

    public void PlayAudioOneShotAtPosition(AudioClip clip, Vector3 position)
    {
        GameObject entityFound = null;
        for (int i = 0; i < audioEntities.Count; i++)
        {
            if (!audioEntities[i].activeInHierarchy)
            {
                entityFound = audioEntities[i];
                break;
            }
        }
        if(entityFound == null)
        { 
            entityFound = Instantiate(audioEntity,transform);
            audioEntities.Add(entityFound);
        }
        entityFound.SetActive(true);
        StartCoroutine(DisableEntity(entityFound, clip, position));
    }

    IEnumerator DisableEntity(GameObject entity, AudioClip clip, Vector3 position)
    {
        entity.transform.position = position;
        entity.GetComponent<AudioSource>().PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        entity.SetActive(false);
    }
}
