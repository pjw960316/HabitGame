using UniRx;
using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    #region 1. Fields

    protected UIManager _uiManager;
    protected UIToastManager _uiToastManager;
    protected PresenterManager _presenterManager;
    
    protected EPopupKey _ePopupKey;

    protected readonly CompositeDisposable _disposables = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        Initialize();

        CreatePresenterByManager();

        BindEvent();
    }

    protected virtual void Initialize()
    {
        _uiManager = UIManager.Instance;
        _uiToastManager = UIToastManager.Instance;
        _presenterManager = PresenterManager.Instance;
    }

    protected abstract void BindEvent();
    protected abstract void CreatePresenterByManager();
    protected abstract void InitializeEPopupKey();

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected virtual void ClosePopup()
    {
        _uiManager.OnClosePopup.OnNext(_ePopupKey);
        _disposables?.Dispose();

        Destroy(gameObject);
    }

    #endregion
}