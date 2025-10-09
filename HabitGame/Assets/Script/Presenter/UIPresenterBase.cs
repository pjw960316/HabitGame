public abstract class UIPresenterBase : PresenterBase
{
    #region 1. Fields

    // refactor
    // 항상 팝업임
    private UIPopupBase _popupBase;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void Initialize(IView view)
    {
        base.Initialize(view);

        _popupBase = _view as UIPopupBase;
        ExceptionHelper.CheckNullException(_popupBase, "_popupBase is null");

        //fix
        _model = SoundManager.Instance.SoundData;
        ExceptionHelper.CheckNullException(_model, "PresenterBase's _model");
    }

    // note
    // 모든 Presenter는 Manager 또는 Model을 통해 
    // 로직 데이터를 본인의 Initialize()에서 초기화 한다.
    // 그 후, 그걸 이용해서 View에 Data를 Inject해서 View를 세팅할 책임이 있다.
    protected abstract void SetView();

    protected virtual void BindEvent()
    {
        //
    }

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