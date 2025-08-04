using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UIToastManager : ManagerBase<UIToastManager>, IManager, IDisposable
{
    #region 1. Fields

    private Transform _toastMessageTarget;
    // default

    #endregion

    #region 2. Properties

    // REFACTOR
    // can be null? - view로 받기 때문에.
    public UIMainCanvas MainCanvas { get; private set; }

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    public void InjectMainCanvas(UIMainCanvas canvas)
    {
        MainCanvas = canvas;
    }

    public void SetModel(IEnumerable<IModel> _list)
    {
    }

    public void ShowToastMessage()
    {
        MainCanvas.ToastMessage.SetActive(true);

        RemoveToastMessage();
    }

    //refactor
    //dispose?
    private void RemoveToastMessage()
    {
        Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ => { MainCanvas.ToastMessage.SetActive(false); });
    }

    #endregion

    #region 5. EventHandlers

    public void Dispose()
    {
    }

    #endregion
}