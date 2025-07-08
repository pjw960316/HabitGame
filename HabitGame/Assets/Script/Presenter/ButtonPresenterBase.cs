// 간단한 버튼은 이거만 써도 된다.

using UniRx;

public class ButtonPresenterBase : PresenterBase
{
    private UIButtonBase _button; // 이 추상화 수준에서의 View를 초기화 한다.

    #region 1. Fields

    // default

    #endregion


    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        if (View is UIButtonBase uiButton)
        {
            _button = uiButton;
        }

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _button.OnClickButton.Subscribe(_ => OpenPopup()).AddTo(Disposable);
    }

    #endregion

    #region 5. EventHandlers

    // TODO : 여기 있는 게 일단 맞는 거 같다. params를 통해 특정 UIPopup을 생성.
    private void OpenPopup()
    {
    }

    #endregion
}