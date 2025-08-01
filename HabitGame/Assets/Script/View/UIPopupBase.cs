using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    private void Awake()
    {
        OnAwake();
    }


    public void BindEvent()
    {
        BindEventInternal();
    }

    protected virtual void BindEventInternal()
    {
    }

    public void OnAwake()
    {
        BindEvent();
    }

    public void CreatePresenterByManager()
    {
        //
    }
}