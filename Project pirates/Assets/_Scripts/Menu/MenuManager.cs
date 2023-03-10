using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    [field: SerializeField] private MenuBase _menuMain { get; set; }
    [field: SerializeField] private MenuBase _menuSettings { get; set; }
    [field: SerializeField] private MenuBase _menuCredits { get; set; }
    [field: SerializeField] private MenuBase _menuPause { get; set; }
    [field: SerializeField] private MenuBase _menuHUD { get; set; }
    private static MenuType _currentMenu;
    private static MenuType _previousMenu;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        InitializeMenus();
        SubscribeToInput();
        if (GameManager.Instance.StartInMenu)
            ShowMenu(MenuType.Main);
        else
            ShowMenu(MenuType.HUD);
    }
    private void InitializeMenus()
    {
        _menuMain.Initialize();
        _menuSettings.Initialize();
        _menuCredits.Initialize();
        _menuPause.Initialize();
        _menuHUD.Initialize();
    }
    public static void ShowMenu(MenuType menu)
    {
        Instance.SetActiveOnMenu(_currentMenu, false);
        if (Instance.SetActiveOnMenu(menu, true))
        {
            _previousMenu = _currentMenu;
            _currentMenu = menu;
        }
        else // fall back to previous menu
            Instance.SetActiveOnMenu(_currentMenu, true);
    }

    public void OnPauseGameInput(InputAction.CallbackContext context)
    {
        Debug.Log("OnPauseGameInput" + " State: " + GameManager.CurrentGameState);
        if (context.performed)
        {
            if (GameManager.CurrentGameState == GameState.InGame)
            {
                ShowMenu(MenuType.Pause);
                GameManager.SwitchToGameState(GameState.Paused);
            }
            else if (GameManager.CurrentGameState == GameState.Paused)
            {
                ShowMenu(MenuType.HUD);
                GameManager.SwitchToGameState(GameState.InGame);
            }
        }
    }

    public static void ShowPreviousMenu()
    {
        ShowMenu(_previousMenu);
    }

    private bool SetActiveOnMenu(MenuType menu, bool active)
    {
        switch (menu)
        {
            case MenuType.Main:
                _menuMain.gameObject.SetActive(active);
                _menuMain.SetSelection();
                break;
            case MenuType.Settings:
                _menuSettings.gameObject.SetActive(active);
                _menuSettings.SetSelection();
                break;
            case MenuType.Credits:
                _menuCredits.gameObject.SetActive(active);
                _menuCredits.SetSelection();
                break;
            case MenuType.Pause:
                _menuPause.gameObject.SetActive(active);
                _menuPause.SetSelection();
                break;
            case MenuType.HUD:
                _menuHUD.gameObject.SetActive(active);
                _menuHUD.SetSelection();
                break;
            default:
                Debug.LogError("MenuManager.SetActiveOnMenu: Invalid menu type");
                return false;
        }
        return true;
    }

    private void SubscribeToInput()
    {
        InputManager.OnPauseGame += OnPauseGameInput;
    }
    private void UnsubscribeFromInput()
    {
        InputManager.OnPauseGame -= OnPauseGameInput;
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            UnsubscribeFromInput();
        }
    }
}

public enum MenuType
{
    Main,
    Settings,
    Credits,
    Pause,
    HUD
}