// 간단한 버튼은 이거만 써도 된다.

using UniRx;
using UnityEngine;

public class ButtonPresenterBase : PresenterBase
{
    private UIButtonBase _button; // 이 추상화 수준에서의 View를 초기화 한다.

    #region 1. Fields

    private UIManager _uiManager;

    //test
    private Canvas _canvas;

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

        _uiManager = UIManager.Instance;
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
        //test string key
        //test 
        Debug.Log("Open?");
        _uiManager.OpenPopupByStringKey("AlarmButton", _canvas.transform);
    }

    //test
    public void SetCanvas(Canvas canvas)
    {
        _canvas = canvas;
    }

    #endregion
}