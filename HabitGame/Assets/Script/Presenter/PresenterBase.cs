using UniRx;

public abstract class PresenterBase : IPresenter
{
    #region 1. Fields

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
        _view = view;
        //fix
        _model = SoundManager.Instance.SoundData;
        
        ExceptionHelper.CheckNullException(_view, "PresenterBase's _view");
        ExceptionHelper.CheckNullException(_model, "PresenterBase's _model");
    }

    #endregion

    #region 4. Methods

    //default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}