using UnityEngine;

public class UIOpenPopupButtonBase : UIButtonBase
{
    #region 1. Fields

    [SerializeField] private EPopupKey ePopupKey;

    #endregion

    #region 2. Properties

    // default

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