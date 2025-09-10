using UniRx;
using UnityEngine;

public abstract class PresenterBase : IPresenter
{
    #region 1. Fields

    protected SoundManager _soundManager;
    protected UIManager _uiManager;
    protected UIToastManager _uiToastManager;
    protected ModelManager _modelManager;
    protected MyCharacterManager _myCharacterManager;
    protected StringManager _stringManager;
    protected PresenterManager _presenterManager;
    
    protected IView _view;
    protected IModel _model;
    protected readonly CompositeDisposable _disposable = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public virtual void Initialize(IView view)
    {
        _soundManager = SoundManager.Instance;
        _uiToastManager = UIToastManager.Instance;
        _uiManager = UIManager.Instance;
        _myCharacterManager = MyCharacterManager.Instance;
        _modelManager = ModelManager.Instance;
        _stringManager = StringManager.Instance;
        _presenterManager = PresenterManager.Instance;
        
        _view = view;
        
        //fix
        _model = SoundManager.Instance.SoundData;
        
        ExceptionHelper.CheckNullException(_view, "PresenterBase's _view");
        ExceptionHelper.CheckNullException(_model, "PresenterBase's _model");
    }

    // note
    // 모든 Presenter는 Manager 또는 Model을 통해 
    // 로직 데이터를 본인의 Initialize()에서 초기화 한다.
    // 그 후, 그걸 이용해서 View에 Data를 Inject해서 View를 세팅할 책임이 있다.
    protected abstract void SetView();

    protected virtual void BindEvent()
    {
        // refactor
        // 코드 정리
        // 모든 Presenter는 닫힐 때 제거 여부를 검사하고 닫아야 한다. 
        // 1Presenter Many View가 되기 때문.
        // PresenterBase를 FieldObject에 대해서도 쓸거냐?
        var uiPopupBase = _view as UIPopupBase;
        uiPopupBase?.OnClose.Do(_ => Debug.Log("SubScribe")).Subscribe(_ => OnClosePopup()).AddTo(_disposable);
        
    }
    
    #endregion

    #region 4. EventHandlers

    // note
    // 하위 타입에서 Presenter를 제거할 지 결정할 책임을 부여한다.
    protected abstract void OnClosePopup();
    
    protected void TerminatePresenter()
    {
        _disposable?.Dispose();
        _presenterManager.TerminatePresenter(this);
    }
    
    // todo 
    // abstract로 _view 연결 끊는 코드 만들자.

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    //

    #endregion
}