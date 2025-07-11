using UniRx;

public abstract class PresenterBase : IPresenter
{
    #region 1. Fields

    protected IView View;
    protected IModel Model;
    protected readonly CompositeDisposable Disposable = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    // refactor
    // 생성 될 때 IView와 IModel을 받도록 책임은 부여했다.
    // 그러나 이걸 콜 하는 건 별개다. 왜냐 부모니까. Default가 아니거든.
    protected PresenterBase()
    {
    }

    // default

    #endregion

    #region 4. Methods

    protected virtual void Initialize()
    {
        
    }
    //protected abstract void CastAbstractModel();
    //protected abstract void CastAbstractView();

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}