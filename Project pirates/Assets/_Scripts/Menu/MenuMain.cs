using UnityEngine;
using UnityEngine.UI;
public class MenuMain : MenuBase
{
    [field: SerializeField] protected Button _buttonStartOrResume { get; set; }
    [field: SerializeField] protected Button _buttonSettings { get; set; }
    [field: SerializeField] protected Button _buttonCredits { get; set; }
    [field: SerializeField] protected Button _buttonQuit { get; set; }

    public override void Initialize()
    {
        base.Initialize();
        AddListeners();
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            _buttonQuit.interactable = false;
#if UNITY_EDITOR
        _buttonQuit.interactable = false;
#endif
    }

    public override void SetSelection()
    {
        _buttonStartOrResume.Select();
    }

    protected virtual void AddListeners()
    {
        _buttonStartOrResume.onClick.AddListener(OnStartOrResume);
        _buttonSettings.onClick.AddListener(OnSettings);
        _buttonCredits.onClick.AddListener(OnCredits);
        _buttonQuit.onClick.AddListener(OnQuit);
    }
    private void OnEnable()
    {
        if (GameManager.CurrentGameState != Gamestate.MainMenu)
            GameManager.SwitchToGameState(Gamestate.MainMenu);
    }
    protected virtual void OnStartOrResume()
    {
        // start game
    }

    private void OnSettings()
    {
        MenuManager.ShowMenu(MenuType.Settings);
    }

    private void OnCredits()
    {
        MenuManager.ShowMenu(MenuType.Credits);
    }

    private void OnQuit()
    {
        Application.Quit();
    }
}