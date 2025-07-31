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

    //Note
    //View를 알면 Model을 알 수 있도록 만들고 싶다.
    public virtual void Initialize(IView view)
    {
        _view = view;

        SetModel();
    }

    #endregion

    #region 4. Methods

    // refactor
    // 지금 모델이 2개 이상일 때 전혀 대응이 되지 않고 있다.
    private void SetModel()
    {
        //test
        //일단 SoundManager로
        _model = SoundManager.Instance.SoundData;
    }

    //default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}