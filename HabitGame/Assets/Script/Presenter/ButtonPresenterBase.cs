using UniRx;

public class ButtonPresenterBase : PresenterBase
{
    #region 1. Fields

    // NOTE
    // 이 추상화 수준에서의 View를 초기화 한다.
    private UIButtonBase _view;
    private UIManager _uiManager;

    #endregion


    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void Initialize(IView view)
    {
        if (view is UIButtonBase uiButton)
        {
            _view = uiButton;
        }

        _uiManager = UIManager.Instance;

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _view.OnClickButton.Subscribe(RequestOpenPopup).AddTo(Disposable);
    }

    #endregion

    #region 5. EventHandlers

    private void RequestOpenPopup(EPopupKey ePopupKey)
    {
        var targetTransform = _view.Canvas.transform;
        _uiManager.OpenPopupByStringKey(ePopupKey,  targetTransform);
    }

    #endregion
}