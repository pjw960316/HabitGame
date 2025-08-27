using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private EStringKey _buttonTextKey;
    [SerializeField] protected Button _button;
    [SerializeField] private bool _isAutoText;

    protected UIManager _uiManager;

    #endregion

    #region 2. Properties

    public Canvas Canvas => _canvas;
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

        // TODO
        // 지금은 Presenter 없이 동작 시킬 계획
        //CreatePresenterByManager();

        // Note
        // Shadowing
        // Script가 UIButtonBase가 붙으면 Base의 BindEvent()가 호출되고
        // Script가 UIOpenPopupButtonBase가 붙어도 Derived의 BindEvent()가 호출되기 바람.
        BindEvent();
    }

    /*private void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<ButtonPresenterBase>(this);
    }*/

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