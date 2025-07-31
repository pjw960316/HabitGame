using UniRx;

public class ButtonPresenterBase : PresenterBase
{
    #region 1. Fields

    // NOTE
    // 이 추상화 수준에서의 View를 초기화 한다.
    private UIButtonBase _uiButtonBase;
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
            _uiButtonBase = uiButton;
        }

        _uiManager = UIManager.Instance;

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _uiButtonBase.OnClickButton.Subscribe(RequestOpenPopup).AddTo(_disposable);
    }

    #endregion

    #region 5. EventHandlers

    private void RequestOpenPopup(EPopupKey ePopupKey)
    {
        var targetTransform = _uiButtonBase.Canvas.transform;
        
        _uiManager.OpenPopupByStringKey(ePopupKey,  targetTransform);
    }

    #endregion
}