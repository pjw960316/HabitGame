using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private EStringKey _buttonTextKey;
    [SerializeField] protected Button Button;
    protected IPresenter Presenter;
    
    #endregion

    #region 2. Properties

    public Canvas Canvas { get; private set; }

    #endregion

    #region 3. Constructor

    public void Awake()
    {
        // Note
        // Overriding
        // Script가 UIButtonBase가 붙으면 Base의 OnAwake()가 호출되고
        // Script가 UIOpenPopupButtonBase가 붙으면 Derived의 OnAwake()가 호출되기 바람.
        OnAwake();
    }

    public virtual void OnAwake()
    {
        // Note
        // Shadowing
        // Script가 UIButtonBase가 붙으면 Base의 BindEvent()가 호출되고
        // Script가 UIOpenPopupButtonBase가 붙어도 Derived의 BindEvent()가 호출되기 바람.
        BindEvent();
        SetButtonText();
    }

    #endregion

    #region 4. Methods

    // Note
    // Virtual로 변경하지 마세요.
    // 모든 상속 구조에서 Binding은 독립적으로 각각 실행되어야 합니다.
    private void BindEvent()
    {
    }

    private void SetButtonText()
    {
        ExceptionHelper.CheckNullException(_buttonText, "buttonText");

        _buttonText.text = StringManager.Instance.GetUIString(_buttonTextKey);
    }

    protected virtual void ConnectPresenter()
    {
        SoundManager.Instance.GetPresenterAfterCreate<ButtonPresenterBase>(this);
    }
    
    #endregion

    #region 5. EventHandlers

    //

    #endregion
}