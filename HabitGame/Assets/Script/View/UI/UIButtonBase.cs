using TMPro;
using UnityEngine;
using UnityEngine.UI;

// note : Button은 Widget이므로 MVP 대신, 오직 View에서 모두 처리해도 무방하다고 생각한다.
public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] protected Button _button;
    [SerializeField] protected bool _isAutoText;
    [SerializeField] protected TextMeshProUGUI _buttonText;
    [SerializeField] protected EStringKey _buttonTextKey;

    protected UIManager _uiManager;
    protected StringManager _stringManager;
    protected SoundManager _soundManager;

    #endregion

    #region 2. Properties

    public Button Button => _button;
    public Button.ButtonClickedEvent OnClick => _button.onClick;

    #endregion

    #region 3. Constructor

    public void Awake()
    {
        Initialize();

        BindEvent();
    }

    // note : public으로 해서, popup에서 제어해야 시점을 명확히 시킨다.
    public virtual void Initialize()
    {
        _uiManager = UIManager.Instance;
        _stringManager = StringManager.Instance;
        _soundManager = SoundManager.Instance;

        UpdateButtonAutoText();
    }


    protected virtual void BindEvent()
    {
        OnClick.AddListener(OnClickButton);
    }

    #endregion

    #region 4. EventHandlers

    protected virtual void OnClickButton()
    {
        _soundManager.PlayCurrentSFX();
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

        _buttonText.text = _stringManager.GetUIString(_buttonTextKey);
    }

    #endregion
}