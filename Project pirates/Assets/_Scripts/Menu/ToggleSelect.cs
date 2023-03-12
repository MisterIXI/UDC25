using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using TMPro;

public class ToggleSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler {

    private AudioClips _audioClips;

    private Toggle _toggle;
    private RectTransform _toggleTransform;

    public Vector2 startSize = new Vector2(50f, 50f);
    public Vector2 selectSize = new Vector2(55f, 55f);

    private bool selected;


    private void Start() 
    {
        _audioClips = SoundManager.AudioClips;
    }
    
    private void Awake() {
        _toggle = GetComponent<Toggle>();
        _toggleTransform = GetComponent<RectTransform>();

        // startSize = _toggleTransform.sizeDelta;
        _toggleTransform.sizeDelta = startSize;
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
        if (_toggle != null && _toggle.name != "Button_Play")
        {
            selected = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _toggle.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;

        SoundManager.Instance.PlayAudioOneShotAtPosition(_audioClips.ButtonHover, Camera.main.transform.position);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
    }


    private void StyleSelected()
    {
        if(_toggleTransform.sizeDelta != selectSize)
        {
            _toggleTransform.sizeDelta = Vector2.MoveTowards(_toggleTransform.sizeDelta, selectSize, 200f * Time.unscaledDeltaTime);
        }
    }

    private void StyleNormal()
    {
        if(_toggleTransform.sizeDelta != startSize)
        {
            _toggleTransform.sizeDelta = Vector2.MoveTowards(_toggleTransform.sizeDelta, startSize, 200f * Time.unscaledDeltaTime);
        }
    }
}