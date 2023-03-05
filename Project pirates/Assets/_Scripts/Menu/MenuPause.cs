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