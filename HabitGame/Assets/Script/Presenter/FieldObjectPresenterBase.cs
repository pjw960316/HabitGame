using UniRx;

public abstract class FieldObjectPresenterBase : IPresenter
{
    #region 1. Fields

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
        _view = view;
    }

    protected virtual void BindEvent()
    {
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}