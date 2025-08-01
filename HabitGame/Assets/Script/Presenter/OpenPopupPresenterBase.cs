public class OpenPopupPresenterBase : ButtonPresenterBase
{
    #region 1. Fields

    // default

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void Initialize(IView view)
    {
        base.Initialize(view);

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
    }

    protected void RequestOpenPopup(EPopupKey ePopupKey)
    {
        var targetTransform = _uiButtonBase.Canvas.transform;
        _uiManager.OpenPopupByStringKey(ePopupKey, targetTransform);
    }

    #endregion

    #region 5. EventHandlers

    protected override void OnClickButton(EPopupKey ePopupKey)
    {
        RequestOpenPopup(ePopupKey);
    }

    #endregion
}