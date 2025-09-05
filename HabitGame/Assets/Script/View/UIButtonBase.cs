using TMPro;
using UnityEngine;
using UnityEngine.UI;

// note
// 현재는 Presenter 붙이지 않을 계획.
public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] protected Button _button;
    [SerializeField] protected bool _isAutoText;
    [SerializeField] protected TextMeshProUGUI _buttonText;
    [SerializeField] protected EStringKey _buttonTextKey;

    protected UIManager _uiManager;
    protected StringManager _stringManager;

    #endregion

    #region 2. Properties

    public Button Button => _button;
    public Button.ButtonClickedEvent OnClick => _button.onClick;

    #endregion

    #region 3. Constructor

    public void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        Initialize();

        BindEvent();
    }

    public virtual void Initialize()
    {
        _uiManager = UIManager.Instance;
        _stringManager = StringManager.Instance;

        UpdateButtonAutoText();
    }


    protected virtual void BindEvent()
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

    protected void UpdateButtonColor(Color color)
    {
        _button.image.color = color;
    }

    protected void UpdateButtonAutoText()
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