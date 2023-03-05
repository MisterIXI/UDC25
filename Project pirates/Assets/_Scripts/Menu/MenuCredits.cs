using UnityEngine;
using UnityEngine.UI;
public class MenuCredits : MenuBase
{
    [field: SerializeField] public Button _buttonBack { get; private set; }

    override public void Initialize()
    {
        base.Initialize();
        AddListeners();
    }

    private void AddListeners()
    {
        _buttonBack.onClick.AddListener(OnBack);
    }
    public override void SetSelection()
    {
        _buttonBack.Select();
    }
    private void OnBack()
    {
        MenuManager.ShowPreviousMenu();
    }

}