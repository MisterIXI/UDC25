using TMPro;
using UnityEngine;
public class TextTweening : MonoBehaviour
{
    public string Text = "";
    private string _currentText = "";
    private TextMeshProUGUI _textMesh;
    private float _characterTimer = 0f;
    private float _timePerChar = 0.05f;
    private bool _isDeleting = true;
    private void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }
    private void FixedUpdate()
    {
        if (Text != _currentText)
        {
            if (_isDeleting)
            {
                if (_timePerChar < 0)
                {
                    _characterTimer = _timePerChar;
                    _textMesh.text = _textMesh.text.Remove(_textMesh.text.Length - 1);
                    _isDeleting = _textMesh.text.Length > 0;
                }
                else
                    _timePerChar -= Time.deltaTime;
            }
            else
            {
                if (_timePerChar < 0)
                {
                    _characterTimer = _timePerChar;
                    _textMesh.text += Text[_textMesh.text.Length];
                    if (_textMesh.text.Length == Text.Length)
                    {
                        _currentText = Text;
                        _isDeleting = true;
                    }
                }
                else
                    _timePerChar -= Time.deltaTime;
            }
        }
    }

}