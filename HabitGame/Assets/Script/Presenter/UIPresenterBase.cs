public abstract class UIPresenterBase : PresenterBase
{
    #region 1. Fields

    private UIPopupBase _popupBase;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void Initialize(IView view)
    {
        base.Initialize(view);
    }

    protected override void InitializeView()
    {
        _popupBase = _view as UIPopupBase;
        ExceptionHelper.CheckNullException(_popupBase, "_popupBase is null");
    }

    // NOTE
    // model은 공용 계층이 일단을 필요없어서 여기서 override 하지 않는다.

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    private void UpdateLivedPopup(EPopupKey ePopupKey)
    {
        _uiManager.RemoveOpenedPopup(ePopupKey);
    }

    // NOTE
    // UI는 Popup 정리 + Presenter 정리
    protected void Close()
    {
        var popupKey = _popupBase.EPopupKey;

        _popupBase.ClosePopup();

        UpdateLivedPopup(popupKey);

        TerminatePresenter();
    }

    #endregion
}
