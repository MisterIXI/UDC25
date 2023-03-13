using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static GameState CurrentGameState { get; private set; } = GameState.InGame;
    public static event Action<GameState, GameState> OnGameStateChanged;
    public static bool IsGameActiveAndPlaying => CurrentGameState == GameState.InGame;
    [field: SerializeField] public bool StartInMenu { get; private set; } = false;
    public static Action<int> OnSceneSwitched;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        OnGameStateChanged += OnGameStateChange;
        SceneManager.activeSceneChanged += SceneSwitch;
    }
    private void Start()
    {
        if (StartInMenu)
            SwitchToGameState(GameState.MainMenu);
        else
            SwitchToGameState(GameState.InGame);
    }
    private void SceneSwitch(Scene oldScene, Scene newScene)
    {
        OnSceneSwitched?.Invoke(newScene.buildIndex);
    }
    public static void SwitchToGameState(GameState newState)
    {
        var oldState = CurrentGameState;
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(oldState, newState);
    }
    public static void LoadGameScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
            SceneManager.LoadScene(1);
    }
    private void OnGameStateChange(GameState oldState, GameState newState)
    {
        // Debug.LogError($"Game State Changed from {oldState} to {newState}");
        // timescale
        switch (newState)
        {
            case GameState.InGame:
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case GameState.MainMenu:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 1;
                SoundManager.FadeToMusic(SoundManager.AudioClips.MenuMusic);
                break;
            case GameState.Paused:
            case GameState.GameOver:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    MainMenu,
    InGame,
    Paused,
    GameOver
}
