using System;
using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    private void Awake()
    {
        BindEvent();
    }

    public void BindEvent()
    {
        BindEventInternal();
    }

    protected virtual void BindEventInternal()
    {
        
    }
}