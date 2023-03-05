using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static Gamestate CurrentGameState { get; private set; }
    public static event Action<Gamestate, Gamestate> OnGameStateChanged;
    public static bool IsGameActiveAndPlaying => CurrentGameState == Gamestate.InGame;
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
            SwitchToGameState(Gamestate.MainMenu);
        else
            SwitchToGameState(Gamestate.InGame);
    }

    public static void SwitchToGameState(Gamestate newState)
    {
        OnGameStateChanged?.Invoke(CurrentGameState, newState);
        CurrentGameState = newState;
    }
    private void OnGameStateChange(Gamestate oldState, Gamestate newState)
    {
        // timescale
        switch (newState)
        {
            case Gamestate.InGame:
                Time.timeScale = 1;
                break;
            case Gamestate.MainMenu:
            case Gamestate.Paused:
            case Gamestate.GameOver:
                Time.timeScale = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum Gamestate
{
    MainMenu,
    InGame,
    Paused,
    GameOver
}
