using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _checkBoxBackgroundImg;
    [SerializeField] private Image _checkBoxCheckMarkImg;

    private UIManager _uiManager;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        OnAwake();
    }

    public void OnAwake()
    {
        Debug.Log("Toggle OnAwake");
        Initialize();

        CreatePresenterByManager();
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
        Debug.Log("Toggle Initialize");
        _uiManager = UIManager.Instance;

        BindEvent();
    }

    private void BindEvent()
    {
        Debug.Log("Toggle BindEvent");
        _toggle.onValueChanged.AddListener(_ =>
        {
            Debug.Log("hi");
        });
    }

    public void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<TogglePresenterBase>(this);
    }

    #endregion

    #region 5. EventHandlers
    
    // default

    #endregion
}