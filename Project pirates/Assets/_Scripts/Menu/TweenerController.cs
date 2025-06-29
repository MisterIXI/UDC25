using UnityEngine;

public class TweenerController : MonoBehaviour
{
    [field: SerializeField] public InteractionTweening InteractTween { get; private set; }
    [field: SerializeField] public InteractionTweening HoldTween { get; private set; }
    [field: SerializeField] public InteractionTweening PauseTween { get; private set; }

    private void Start()
    {
        SetPauseTween();
        PlayerInteract.OnInteractableTargetChanged += OnInteractTargetChanged;
        HoldObject.OnPotentialRigidbodyChanged += OnHoldTargetChanged;
        OnInteractTargetChanged();
        OnHoldTargetChanged();
    }

    public void OnInteractTargetChanged()
    {
        if (PlayerInteract.PossibleInteractableObject != null)
        {
            InteractTween.TextTweening.SetText(PlayerInteract.PossibleInteractableObject.Data());
            InteractTween.ShowInList = true;
        }
        else
        {
            InteractTween.TextTweening.SetText(" ");
            InteractTween.ShowInList = false;
        }
    }

    public void OnHoldTargetChanged()
    {
        if (HoldObject.IsHoldingObject)
            HoldTween.TextTweening.SetText("Drop Object");
        else
        {
            if (HoldObject.PotentialRigidbody != null)
            {
                HoldTween.TextTweening.SetText("Hold Object");
                HoldTween.ShowInList = true;
            }
            else
            {
                HoldTween.ShowInList = false;
                HoldTween.TextTweening.SetText(" ");
            }
        }
    }

    public void SetPauseTween()
    {
        PauseTween.ShowInList = true;
    }
}