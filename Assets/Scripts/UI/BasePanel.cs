using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    public virtual void Init() { }
    public abstract void Show(float delay = 0f);
    public abstract void Hide(float delay = 0f);
    public abstract string GetId();
}
