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

    public void OnAwake()
    {
        Initialize();

        CreatePresenterByManager();
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
        _uiManager = UIManager.Instance;
    }

    public void CreatePresenterByManager()
    {
        // test
        _uiManager.CreatePresenter<ButtonPresenterBase>(this);
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}