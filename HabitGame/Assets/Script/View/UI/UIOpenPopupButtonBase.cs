using UnityEngine;

public class UIOpenPopupButtonBase : UIButtonBase
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] private EPopupKey _ePopupKey;
    
    private Transform _canvasTransform;

    #endregion

    #region 2. Properties

    private Canvas Canvas => _canvas;

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        base.Initialize();

        _canvasTransform = Canvas.transform;
    }

    protected sealed override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    #region 4. Methods

    //

    #endregion

    #region 5. EventHandlers
    
    // NOTE
    // Popup을 여는 게 클래스의 핵심 책임이다.
    // 두 인자만 알면 Manager를 통해 켤 수 있다.
    protected override void OnClickButton()
    {
        base.OnClickButton();

        if (_uiManager.IsAnyPopupOpened())
        {
            return;
        }

        _uiManager.OpenPopupByStringKey(_ePopupKey, _canvasTransform);
    }

    #endregion
}
