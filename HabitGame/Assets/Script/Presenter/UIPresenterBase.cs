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

        //fix : 없앨듯?
        _model = SoundManager.Instance.SoundData;
        ExceptionHelper.CheckNullException(_model, "PresenterBase's _model");
    }

    protected override void InitializeView()
    {
        _popupBase = _view as UIPopupBase;
        ExceptionHelper.CheckNullException(_popupBase, "_popupBase is null");
    }

    // note : model은 공용 계층이 일단을 필요없어서 여기서 override 하지 않는다.

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    private void RequestUpdateLivedPopup(EPopupKey ePopupKey)
    {
        _uiManager.RemoveOpenedPopup(ePopupKey);
    }

    #endregion

    #region 6. Methods

    // note
    // UI는 Popup 정리 + Presenter 정리
    protected void Close()
    {
        var popupKey = _popupBase.EPopupKey;

        _popupBase.ClosePopup();

        RequestUpdateLivedPopup(popupKey);

        TerminatePresenter();
    }

    #endregion
}