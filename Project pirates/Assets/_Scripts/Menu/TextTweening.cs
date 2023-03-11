using TMPro;
using UnityEngine;
public class TextTweening : MonoBehaviour
{
    public string Text { get; private set; }
    private TextMeshProUGUI _textMesh;
    private float _characterTimer = 0f;
    private bool _isDeleting = true;
    private bool _isTweening = false;
    private PlayerSettings _playerSettings;
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        _playerSettings = SettingsManager.PlayerSettings;
    }
    public void SetText(string text)
    {
        if (text == null || _textMesh.text == text)
            return;
        Text = text;
        _isDeleting = _textMesh.text.Length > 0;
        _isTweening = true;
        _characterTimer = _playerSettings.UiTextCharacterTweening;
    }
    private void Tween(float timeDelta)
    {
        _characterTimer -= timeDelta;
        if (_characterTimer < 0)
        {
            _characterTimer = _playerSettings.UiTextCharacterTweening;
            if (_isDeleting)
            {
                _textMesh.text = _textMesh.text.Remove(_textMesh.text.Length - 1);
                _isDeleting = _textMesh.text.Length > 0;
            }
            else
            {
                if (Text.Length == 0)
                {
                    _isTweening = false;
                    return;
                }
                _textMesh.text += Text[_textMesh.text.Length];
                _isTweening = _textMesh.text.Length != Text.Length;
            }
        }
    }
    private void FixedUpdate()
    {
        if (_isTweening)
            Tween(Time.fixedDeltaTime);
    }

}