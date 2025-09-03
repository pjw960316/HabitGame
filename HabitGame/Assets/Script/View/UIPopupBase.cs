using UniRx;
using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    #region 1. Fields

    protected StringManager _stringManager;
    protected UIManager _uiManager;
    protected SoundManager _soundManager;
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

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected virtual void Initialize()
    {
        _stringManager = StringManager.Instance;
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;
    }

    protected abstract void BindEvent();
    protected abstract void CreatePresenterByManager();

    protected void ClosePopup()
    {
        //refactor
        Destroy(gameObject);
    }

    #endregion
}