using UnityEngine;

public class UIOpenPopupButtonBase : UIButtonBase
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] private EPopupKey _ePopupKey;
    private Transform _canvasTransform;

    #endregion

    #region 2. Properties

    private Canvas Canvas => _canvas;

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }

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
        if (_uiManager.IsAnyPopupOpened())
        {
            return;
        }

        _uiManager.OpenPopupByStringKey(_ePopupKey, _canvasTransform);
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}