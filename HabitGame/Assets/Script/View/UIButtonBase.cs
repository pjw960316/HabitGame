using TMPro;
using UnityEngine;
using UnityEngine.UI;

// note
// 현재는 Presenter 붙이지 않을 계획.
public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields
    
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private EStringKey _buttonTextKey;
    [SerializeField] protected Button _button;
    [SerializeField] private bool _isAutoText;

    protected UIManager _uiManager;

    #endregion

    #region 2. Properties

    public Button Button => _button;

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
        Initialize();

        // Note
        // Shadowing
        // Script가 UIButtonBase가 붙으면 Base의 BindEvent()가 호출되고
        // Script가 UIOpenPopupButtonBase가 붙어도 Derived의 BindEvent()가 호출되기 바람.
        BindEvent();
    }

    private void Initialize()
    {
        _uiManager = UIManager.Instance;

        InitializeButtonText();
    }

    protected void BindEvent()
    {
        Button.onClick.AddListener(OnClickButton);
    }

    #endregion

    #region 4. EventHandlers

    protected virtual void OnClickButton()
    {
        //
    }

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    private void InitializeButtonText()
    {
        if (!_isAutoText)
        {
            return;
        }

        ExceptionHelper.CheckNullException(_buttonText, "buttonText");

        _buttonText.text = StringManager.Instance.GetUIString(_buttonTextKey);
    }

    #endregion
}