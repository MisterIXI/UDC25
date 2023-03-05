using UnityEngine;
public abstract class MenuBase : MonoBehaviour
{
    public abstract void SetSelection();
    public virtual void Initialize() { }
}