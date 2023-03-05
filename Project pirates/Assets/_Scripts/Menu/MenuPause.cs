using UnityEngine;
using UnityEngine.UI;
public class MenuPause : MenuMain
{
    [field: SerializeField] public Button _buttonMenu { get; private set; }
    protected override void OnStartOrResume()
    {
        // resume game
        GameManager.SwitchToGameState(Gamestate.InGame);
        MenuManager.ShowMenu(MenuType.HUD);
    }
    override protected void AddListeners()
    {
        base.AddListeners();
        _buttonMenu.onClick.AddListener(OnMenu);
    }

    private void OnMenu()
    {
        // go to main menu
        GameManager.SwitchToGameState(Gamestate.MainMenu);
        MenuManager.ShowMenu(MenuType.Main);
    }
}