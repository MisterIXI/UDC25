using UnityEngine;
using UnityEngine.UI;
public class InteractionTweening : MonoBehaviour
{
    public Vector2 ShowHideWidth;
    public Vector2 ShowHideHeight;
    private float _tweenProgress = 0f;
    private float _tweenDuration = 0.5f;
    private LayoutElement _layoutElement;
    public bool ShowInList = false;
    private void Start()
    {
        _layoutElement = GetComponent<LayoutElement>();
        if (ShowInList)
            _tweenProgress = 2f;
    }
    private void FixedUpdate()
    {
        if (ShowInList && _tweenProgress < 2f)
        {
            _tweenProgress += Time.deltaTime / _tweenDuration;
            Tween();
        }
        else if (!ShowInList && _tweenProgress > 0f)
        {
            _tweenProgress -= Time.deltaTime / _tweenDuration;
            Tween();
        }
    }

    private void Tween()
    {
        if (_tweenProgress < 1f)
            _layoutElement.minWidth = Mathf.Lerp(ShowHideWidth.x, ShowHideWidth.y, _tweenProgress);
        else
            _layoutElement.minHeight = Mathf.Lerp(ShowHideHeight.x, ShowHideHeight.y, _tweenProgress - 1f);
    }
}