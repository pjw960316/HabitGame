using System;
using UniRx;
using UnityEngine;

public class UIOpenPopupButtonBase : UIButtonBase
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] private EPopupKey _ePopupKey;
    private Transform _canvasTransform;
    private readonly Subject<UIPopupBase> _onPopupOpened = new();

    #endregion

    #region 2. Properties

    private Canvas Canvas => _canvas;
    public IObservable<UIPopupBase> OnPopupOpened => _onPopupOpened;

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        base.Initialize();

        _canvasTransform = Canvas.transform;
    }

    protected sealed override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    #region 4. Methods

    protected override void OnClickButton()
    {
        base.OnClickButton();

        if (_uiManager.IsAnyPopupOpened())
        {
            return;
        }

        var popup = _uiManager.OpenPopupByStringKey(_ePopupKey, _canvasTransform);
        if (popup != null)
        {
            _onPopupOpened.OnNext(popup);
        }
    }

    #endregion

    #region 5. EventHandlers

    private void OnDestroy()
    {
        _onPopupOpened.OnCompleted();
        _onPopupOpened.Dispose();
    }

    #endregion
}
