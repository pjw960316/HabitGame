// 간단한 버튼은 이거만 써도 된다.

using UniRx;

public class ButtonPresenterBase : PresenterBase
{
    // NOTE
    // 이 추상화 수준에서의 View를 초기화 한다.
    private UIButtonBase _view;

    #region 1. Fields

    private UIManager _uiManager;

    #endregion


    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        if (View is UIButtonBase uiButton)
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
        _view.OnClickButton.Subscribe(_ => RequestOpenPopup()).AddTo(Disposable);
    }

    #endregion

    #region 5. EventHandlers

    private void RequestOpenPopup()
    {
        var targetTransform = _view.GetCanvas().transform;
        _uiManager.OpenPopupByStringKey("AlarmButton", targetTransform);
    }

    #endregion
}