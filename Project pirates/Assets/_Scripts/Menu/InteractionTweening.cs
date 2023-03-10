using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InteractionTweening : MonoBehaviour
{
    public Vector2 ShowHideWidth;
    public Vector2 ShowHideHeight;
    [field: SerializeField] private bool _startShowInList;
    public float TweenProgress { get; private set; } = 2f;
    private LayoutElement _layoutElement;
    private bool _showInList = true;
    public bool ShowInList
    {
        get { return _showInList; }
        set
        {
            IsTweening = IsTweening || _showInList != value;
            _showInList = value;
        }
    }
    [field: SerializeField] public TextTweening TextTweening { get; private set; }
    private PlayerSettings _playerSettings;
    public bool IsTweening { get; private set; } = false;
    [field: SerializeField] private TextMeshProUGUI _keyboardPrompt { get; set; }
    [field: SerializeField] private TextMeshProUGUI _gamepadPrompt { get; set; }
    private void Awake()
    {
        if (_startShowInList)
        {
            TweenProgress = 2f;
            _showInList = true;
        }
        else
        {
            TweenProgress = 0f;
            _showInList = false;
        }
    }
    private void Start()
    {
        _playerSettings = SettingsManager.PlayerSettings;
        _layoutElement = GetComponent<LayoutElement>();
        InputManager.OnControlSchemeChanged += SwapButtonPromptForLayout;
        SwapButtonPromptForLayout();
    }
    private void SwapButtonPromptForLayout()
    {
        if (InputManager.IsKeyboardAndMouse)
        {
            _keyboardPrompt.gameObject.SetActive(true);
            _gamepadPrompt.gameObject.SetActive(false);
        }
        else
        {
            _keyboardPrompt.gameObject.SetActive(false);
            _gamepadPrompt.gameObject.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        if (IsTweening)
        {
            var oldTween = TweenProgress;
            if (!ShowInList)
            {

                if (TweenProgress < 1f)
                    TweenProgress += Time.fixedDeltaTime / _playerSettings.UiVertSlideTween;
                else
                    TweenProgress += Time.fixedDeltaTime / _playerSettings.UiSideSlideTween;
                Tween();
            }
            else if (ShowInList)
            {
                if (TweenProgress < 1f)
                    TweenProgress -= Time.fixedDeltaTime / _playerSettings.UiVertSlideTween;
                else
                    TweenProgress -= Time.fixedDeltaTime / _playerSettings.UiSideSlideTween;
                Tween();
            }
            // if (oldTween != TweenProgress)
            //     Debug.Log($"Tween of {name}: {oldTween} -> {TweenProgress}");
        }
    }

    private void Tween()
    {
        TweenProgress = Mathf.Clamp(TweenProgress, 0f, 2f);
        var oldHeight = _layoutElement.minHeight;
        _layoutElement.minWidth = Mathf.Lerp(ShowHideWidth.x, ShowHideWidth.y, TweenProgress);
        _layoutElement.minHeight = Mathf.Lerp(ShowHideHeight.x, ShowHideHeight.y, TweenProgress - 1f);
        // if (oldHeight != _layoutElement.minHeight)
        //     Debug.Log($"Height: {oldHeight} -> {_layoutElement.minHeight}");
        if ((ShowInList && TweenProgress == 0f) || (!ShowInList && TweenProgress == 2f))
            IsTweening = false;
    }


}