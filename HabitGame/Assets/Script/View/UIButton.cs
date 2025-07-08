using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIButton : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] protected Button Button;
    [SerializeField] protected TextMeshProUGUI ButtonText;

    protected UIManager UIManager;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected virtual void Awake()
    {
        Initialize();
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
        UIManager = UIManager.Instance;
        
        BindEvent();
    }

    protected virtual void BindEvent()
    {
    }

    #endregion

    #region 5. EventHandlers

    protected abstract void OnClicked();

    #endregion
}