using UniRx;

public abstract class PresenterBase : IPresenter
{
    #region 1. Fields

    protected SoundManager _soundManager;
    protected UIManager _uiManager;
    protected UIToastManager _uiToastManager;
    protected ModelManager _modelManager;
    protected MyCharacterManager _myCharacterManager;
    protected StringManager _stringManager;
    protected PresenterManager _presenterManager;
    protected MockServerManager _serverManager;

    protected IView _view;
    protected IModel _model;

    protected readonly CompositeDisposable _disposable = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public virtual void Initialize(IView view)
    {
        _soundManager = SoundManager.Instance;
        _uiToastManager = UIToastManager.Instance;
        _uiManager = UIManager.Instance;
        _myCharacterManager = MyCharacterManager.Instance;
        _modelManager = ModelManager.Instance;
        _stringManager = StringManager.Instance;
        _presenterManager = PresenterManager.Instance;
        _serverManager = MockServerManager.Instance;
        _view = view;

        InitializeView();
        InitializeModel();
    }

    public abstract void SetView();

    public abstract void BindEvent();

    protected abstract void InitializeView();

    protected abstract void InitializeModel();

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 
    protected void TerminatePresenter()
    {
        DisposeCompositeDisposables();

        _presenterManager.TerminatePresenter(this);
    }

    // note
    // 최상단의 _disposable은 항상 base를 호출해서 Dispose 시키고
    // Derived Type에서 각각 생성한 disposable들은 override된 DisposeCompositeDisposables()에서 호출되도록 구현
    protected virtual void DisposeCompositeDisposables()
    {
        _disposable?.Dispose();
    }

    #endregion
}