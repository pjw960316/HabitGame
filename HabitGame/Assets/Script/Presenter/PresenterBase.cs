using UniRx;

public abstract class PresenterBase : IPresenter
{
    #region 1. Fields

    protected SoundManager _soundManager;
    protected UIToastManager _uiToastManager;
    protected DataManager _dataManager;
    protected MyCharacterManager _myCharacterManager;
    
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
        _myCharacterManager = MyCharacterManager.Instance;
        _dataManager = DataManager.Instance;
        
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
    

    #endregion

    #region 4. Methods

    //default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}