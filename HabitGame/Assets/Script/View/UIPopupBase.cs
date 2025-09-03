using UniRx;
using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    #region 1. Fields

    protected UIManager _uiManager;
    protected UIToastManager _uiToastManager;

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
    }

    protected abstract void BindEvent();
    protected abstract void CreatePresenterByManager();

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
        _disposables?.Dispose();

        Destroy(gameObject);
    }

    #endregion
}