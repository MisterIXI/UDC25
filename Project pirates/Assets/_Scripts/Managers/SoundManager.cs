using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [field: SerializeField] private AudioClips _audioClips;
    public static AudioClips AudioClips => Instance._audioClips;
    [SerializeField] int amountOfAudioEntities;
    [SerializeField] GameObject audioEntity;
    private AudioSource _cameraAudioSource;
    List<GameObject> audioEntities;
    private PlayerSettings _playerSettings;
    public static SoundManager Instance;
    public static AudioClip CurrentMusic;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    private void OnMusicVolumeChanged(float newVolume)
    {
        _cameraAudioSource.volume = newVolume;
    }
    private void Start()
    {
        _playerSettings = SettingsManager.PlayerSettings;
        audioEntities = new List<GameObject>();
        for (int i = 0; i < amountOfAudioEntities; i++)
        {
            GameObject spawnedEntity = Instantiate(audioEntity, transform);
            spawnedEntity.SetActive(false);
            audioEntities.Add(spawnedEntity);
        }
        _cameraAudioSource = Camera.main.gameObject.AddComponent<AudioSource>();
        _cameraAudioSource.loop = true;
        _cameraAudioSource.volume = _playerSettings.MusicVolume;
    }
    public static void FadeToMusic(AudioClip clip)
    {
        Instance.StopAllCoroutines();
        CurrentMusic = clip;
        Instance.StartCoroutine(Instance.FadeToMusicCoroutine(clip));
    }

    private IEnumerator FadeToMusicCoroutine(AudioClip clip)
    {
        yield return null;
        float startMusicVolume = _cameraAudioSource.volume;
        while (_cameraAudioSource.volume > 0)
        {
            _cameraAudioSource.volume -= startMusicVolume * Time.deltaTime;
            yield return null;
        }
        _cameraAudioSource.Stop();
        _cameraAudioSource.clip = clip;
        _cameraAudioSource.Play();
        while (_cameraAudioSource.volume < _playerSettings.MusicVolume)
        {
            _cameraAudioSource.volume += _playerSettings.MusicVolume * Time.deltaTime;
            yield return null;
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
        if (entityFound == null)
        {
            entityFound = Instantiate(audioEntity, transform);
            audioEntities.Add(entityFound);
        }
        entityFound.SetActive(true);
        StartCoroutine(DisableEntity(entityFound, clip, position));
    }

    IEnumerator DisableEntity(GameObject entity, AudioClip clip, Vector3 position)
    {
        entity.transform.position = position;
        entity.GetComponent<AudioSource>().PlayOneShot(clip, _playerSettings.SfxVolume);
        yield return new WaitForSeconds(clip.length);
        entity.SetActive(false);
    }
}
