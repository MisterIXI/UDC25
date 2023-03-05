using UnityEngine;
using UnityEngine.UI;
public class MenuPause : MenuMain
{
    [field: SerializeField] public Button _buttonMenu { get; private set; }
    protected override void OnStartOrResume()
    {
        // resume game
        GameManager.SwitchToGameState(GameState.InGame);
        MenuManager.ShowMenu(MenuType.HUD);
    }
    override protected void AddListeners()
    {
        base.AddListeners();
        _buttonMenu.onClick.AddListener(OnMenu);
    }
    protected override void DisableQuitButton()
    {
        _buttonQuit.interactable = false;
        Button bottomButton = _buttonMenu;
        Button topButton = _buttonStartOrResume;
        var topNav = topButton.navigation;
        topNav.selectOnUp = bottomButton;
        topButton.navigation = topNav;
        var bottomNav = bottomButton.navigation;
        bottomNav.selectOnDown = topButton;
        bottomButton.navigation = bottomNav;
    }
    protected override void OnEnable()
    {
        // do nothing
    }
    private void OnMenu()
    {
        // go to main menu
        GameManager.SwitchToGameState(GameState.MainMenu);
        MenuManager.ShowMenu(MenuType.Main);
    }
}