using UnityEngine;

public class UIOpenPopupButtonBase : UIButtonBase
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] private EPopupKey ePopupKey;

    #endregion

    #region 2. Properties

    private Canvas Canvas => _canvas;

    #endregion

    #region 3. Constructor

    public override void OnAwake()
    {
        base.OnAwake();
    }

    #endregion

    #region 4. Methods

    protected override void OnClickButton()
    {
        var targetTransform = Canvas.transform;
        _uiManager.OpenPopupByStringKey(ePopupKey, targetTransform);
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}