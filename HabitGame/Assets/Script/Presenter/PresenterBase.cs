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

    //Note
    //View를 알면 Model을 알 수 있도록 만들고 싶다.
    public virtual void Initialize(IView view)
    {
        View = view;

        SetModel();
    }

    #endregion

    #region 4. Methods

    private void SetModel()
    {
        //test
        //일단 SoundManager로
        Model = SoundManager.Instance.SoundData;
    }

    //default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}