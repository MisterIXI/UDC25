using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using TMPro;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler {

    private Button _button;
    private RectTransform _buttonTransform;

    public Vector2 startSize = new Vector2(300f, 85f);
    public Vector2 selectSize = new Vector2(330f, 93.5f);

    private bool selected;

    private void Awake() {
        _button = GetComponent<Button>();
        _buttonTransform = GetComponent<RectTransform>();

        // startSize = _buttonTransform.sizeDelta;
        _buttonTransform.sizeDelta = startSize;
    }

    void Update() 
    {
        if(selected)
        {
            StyleSelected();
        }
        else
        {
            StyleNormal();
        }
    }

    private void OnEnable() {
        if (_button != null && _button.name != "Button_Play")
        {
            _button.Select();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
    }


    private void StyleSelected()
    {
        if(_buttonTransform.sizeDelta != selectSize)
        {
            _buttonTransform.sizeDelta = Vector2.MoveTowards(_buttonTransform.sizeDelta, selectSize, 200f * Time.unscaledDeltaTime);
        }
    }

    private void StyleNormal()
    {
        if(_buttonTransform.sizeDelta != startSize)
        {
            _buttonTransform.sizeDelta = Vector2.MoveTowards(_buttonTransform.sizeDelta, startSize, 200f * Time.unscaledDeltaTime);
        }
    }
}