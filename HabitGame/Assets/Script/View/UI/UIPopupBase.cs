using System;
using UniRx;
using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    #region 1. Fields

    protected UIManager _uiManager;
    protected UIToastManager _uiToastManager;
    protected PresenterManager _presenterManager;
    protected CameraManager _cameraManager;

    protected EPopupKey _ePopupKey;

    protected readonly CompositeDisposable _disposables = new();
    private readonly Subject<Unit> _onCloseRequest = new();

    #endregion

    #region 2. Properties

    public IObservable<Unit> OnCloseRequest => _onCloseRequest;
    public EPopupKey EPopupKey => _ePopupKey;

    #endregion

    #region 3. Constructor

    
    private void Awake()
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
        _cameraManager = CameraManager.Instance;
    }

    protected abstract void BindEvent();
    protected abstract void CreatePresenterByManager();
    protected abstract void InitializeEPopupKey();

    #endregion

    #region 4. EventHandlers

    // note : Presenter가 호출한다.
    public void ClosePopup()
    {
        _disposables?.Dispose();

        // refactor
        // 이거 빨리 껐다 키면 null 뜸
        Destroy(gameObject);
    }

    #endregion

    #region 5. Request Methods

    //

    #endregion
}