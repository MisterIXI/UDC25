using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using TMPro;

public class ToggleSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler {

    private Toggle _slider;
    private RectTransform _sliderTransform;

    public Vector2 startSize = new Vector2(50f, 50f);
    public Vector2 selectSize = new Vector2(55f, 55f);

    private bool selected;

    private void Awake() {
        _slider = GetComponent<Toggle>();
        _sliderTransform = GetComponent<RectTransform>();

        // startSize = _sliderTransform.sizeDelta;
        _sliderTransform.sizeDelta = startSize;
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
        if (_slider != null && _slider.name != "Button_Play")
        {
            selected = false;
        }
        else
        {
            _slider.Select();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _slider.Select();
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
        if(_sliderTransform.sizeDelta != selectSize)
        {
            _sliderTransform.sizeDelta = Vector2.MoveTowards(_sliderTransform.sizeDelta, selectSize, 200f * Time.unscaledDeltaTime);
        }
    }

    private void StyleNormal()
    {
        if(_sliderTransform.sizeDelta != startSize)
        {
            _sliderTransform.sizeDelta = Vector2.MoveTowards(_sliderTransform.sizeDelta, startSize, 200f * Time.unscaledDeltaTime);
        }
    }
}