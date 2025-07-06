using UniRx;

public class AlarmPresenter : IPresenter
{
    #region 1. Fields

    // REFACTOR
    // 언젠가 view : Presenter = 다 : 1이 될 때
    // 이걸 Interface로 받아야 한다.
    private readonly UIAlarmPopup _view;
    private readonly IModel _model;
    
    private readonly CompositeDisposable _disposable = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public AlarmPresenter(IView view, IModel model)
    {
        _view = view as UIAlarmPopup;
        _model = model;

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _view.OnSoundButtonClicked.Subscribe(unit => OpenPopup()).AddTo(_disposable);
    }

    #endregion

    #region 5. EventHandlers

    private void OpenPopup()
    {
    }

    #endregion
}