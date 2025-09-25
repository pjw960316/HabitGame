using UniRx;

public abstract class UIPresenterBase : PresenterBase
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void Initialize(IView view)
    {
        base.Initialize(view);

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
        var popup = _view as UIPopupBase;
        popup?.OnClose.Subscribe(_ => OnClosePopup()).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers
    
    // refactor
    // 이거 왜 abstract지?
    protected abstract void OnClosePopup();

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    //

    #endregion
}