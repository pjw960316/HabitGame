using UniRx;

public class PresenterBase : IPresenter
{
    #region 1. Fields

    protected SoundManager _soundManager;
    protected UIManager _uiManager;
    protected UIToastManager _uiToastManager;
    protected ModelManager _modelManager;
    protected MyCharacterManager _myCharacterManager;
    protected StringManager _stringManager;
    protected PresenterManager _presenterManager;

    // note : View와 Model은 하위 타입에서 캐스팅 해서 사용
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

        _view = view;
        ExceptionHelper.CheckNullException(_view, "PresenterBase's _view");
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected void TerminatePresenter()
    {
        _disposable?.Dispose();
        _presenterManager.TerminatePresenter(this);
    }

    #endregion
}