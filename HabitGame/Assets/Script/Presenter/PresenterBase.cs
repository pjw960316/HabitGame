using UniRx;

public abstract class PresenterBase : IPresenter
{
    #region 1. Fields

    protected SoundManager _soundManager;
    protected UIManager _uiManager;
    protected UIToastManager _uiToastManager;
    protected DataManager _dataManager;
    protected MyCharacterManager _myCharacterManager;
    protected StringManager _stringManager;
    
    protected IView _view;
    protected IModel _model;
    protected readonly CompositeDisposable _disposable = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public virtual void Initialize(IView view)
    {
        _soundManager = SoundManager.Instance;
        _uiToastManager = UIToastManager.Instance;
        _uiManager = UIManager.Instance;
        _myCharacterManager = MyCharacterManager.Instance;
        _dataManager = DataManager.Instance;
        _stringManager = StringManager.Instance;
        
        _view = view;
        
        //fix
        _model = SoundManager.Instance.SoundData;
        
        ExceptionHelper.CheckNullException(_view, "PresenterBase's _view");
        ExceptionHelper.CheckNullException(_model, "PresenterBase's _model");
    }

    // note
    // 모든 Presenter는 Manager 또는 Model을 통해 
    // 로직 데이터를 본인의 Initialize()에서 초기화 한다.
    // 그 후, 그걸 이용해서 View에 Data를 Inject해서 View를 세팅할 책임이 있다.
    protected abstract void SetView();
    protected abstract void BindEvent();


    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // todo
    // closePresenter
    // 여기서 Disposable
    public void DestroyPresenter()
    {
        _disposable?.Dispose();

        //나를 null로? 뭐 여튼 정리 코드 필요함.
    }

    #endregion
}