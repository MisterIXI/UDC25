using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static GameState CurrentGameState { get; private set; }
    public static event Action<GameState, GameState> OnGameStateChanged;
    public static bool IsGameActiveAndPlaying => CurrentGameState == GameState.InGame;
    [field: SerializeField] public bool StartInMenu { get; private set; } = false;
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
    }
    private void Start()
    {
        if (StartInMenu)
            SwitchToGameState(GameState.MainMenu);
        else
            SwitchToGameState(GameState.InGame);
    }

    public static void SwitchToGameState(GameState newState)
    {
        OnGameStateChanged?.Invoke(CurrentGameState, newState);
        CurrentGameState = newState;
    }
    private void OnGameStateChange(GameState oldState, GameState newState)
    {
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
