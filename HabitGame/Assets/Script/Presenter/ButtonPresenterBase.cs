// 간단한 버튼은 이거만 써도 된다.

using UniRx;
using UnityEngine;

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
    // default

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _button.OnClickButton.Subscribe(_ => OpenPopup()).AddTo(Disposable);
    }

    #endregion

    #region 5. EventHandlers

    private void OpenPopup()
    {
    }

    #endregion
}