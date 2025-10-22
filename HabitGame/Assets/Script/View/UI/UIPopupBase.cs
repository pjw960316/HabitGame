using System;
using UniRx;
using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    #region 1. Fields

    protected UIToastManager _uiToastManager;
    protected PresenterManager _presenterManager;
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
        InitializeEPopupKey();
        
        Initialize();
        
        CreatePresenterByManager();
        
        // note : view에서는 최상단 호출
        BindEvent(); 
    }

    protected virtual void Initialize()
    {
        _uiToastManager = UIToastManager.Instance;
        _presenterManager = PresenterManager.Instance;
    }
    protected abstract void InitializeEPopupKey();
    protected abstract void CreatePresenterByManager();
    protected abstract void BindEvent();
    

    #endregion

    #region 4. EventHandlers

    // note : Presenter가 호출한다.
    public void ClosePopup()
    {
        _disposables?.Dispose();

        Destroy(gameObject);
    }

    #endregion

    #region 5. Request Methods

    //

    #endregion
}