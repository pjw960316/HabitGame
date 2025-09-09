using System;
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
    private readonly Subject<Unit> _onClose = new();

    #endregion

    #region 2. Properties
    public IObservable<Unit> OnClose => _onClose;
    public EPopupKey EPopupKey => _ePopupKey;

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

    // note
    // 반드시 Presenter를 통해 Close 되어야 합니다.
    // 항상 닫을 때 Presenter를 정리해야 하는 지 아닌 지 
    // Presenter에게 판단을 넘겨야 한다.
    
    // refactor
    // 내부에서 close하는 거 다 찾아서 제거
    public void ClosePopup()
    {
        _uiManager.OnClosePopup.OnNext(_ePopupKey);
        _onClose.OnNext(default);
        
        _disposables?.Dispose();
        
        Destroy(gameObject);
    }

    #endregion
}