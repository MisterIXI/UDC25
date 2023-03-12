using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using TMPro;

public class SliderSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler {

    private Slider _slider;

    private RectTransform backgroundTransform;
    public Vector2 backgroundStartSize = new Vector2(0f, 0f);
    public Vector2 backgroundSelectSize = new Vector2(0f, 3f);

    private RectTransform fillTransform;
    public Vector2 fillStartSize = new Vector2(10f ,0f);
    public Vector2 fillSelectSize = new Vector2(10f, 3f);

    private bool selected;

    private void Awake() {
        _slider = GetComponent<Slider>();

        backgroundTransform = transform.Find("Background").GetComponent<RectTransform>();
        backgroundTransform.sizeDelta = backgroundStartSize;

        fillTransform = transform.Find("Fill Area").GetComponent<RectTransform>();
        fillTransform.sizeDelta = fillStartSize;
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
        if(backgroundTransform.sizeDelta != backgroundSelectSize)
        {
            backgroundTransform.sizeDelta = Vector2.MoveTowards(backgroundTransform.sizeDelta, backgroundSelectSize, 200f * Time.unscaledDeltaTime);
        }

        if(fillTransform.sizeDelta != fillSelectSize)
        {
            fillTransform.sizeDelta = Vector2.MoveTowards(fillTransform.sizeDelta, fillSelectSize, 200f * Time.unscaledDeltaTime);
        }
    }

    private void StyleNormal()
    {
        if(backgroundTransform.sizeDelta != backgroundStartSize)
        {
            backgroundTransform.sizeDelta = Vector2.MoveTowards(backgroundTransform.sizeDelta, backgroundStartSize, 200f * Time.unscaledDeltaTime);
        }

        if(fillTransform.sizeDelta != fillStartSize)
        {
            fillTransform.sizeDelta = Vector2.MoveTowards(fillTransform.sizeDelta, fillStartSize, 200f * Time.unscaledDeltaTime);
        }
    }
}