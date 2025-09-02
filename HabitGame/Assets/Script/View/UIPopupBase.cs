using UniRx;
using UnityEngine;

// note
// popup에서 Toast를 출력할 책임을 부여해야 하는가?
// 일단은 했다.
public abstract class UIPopupBase : MonoBehaviour, IView
{
    #region 1. Fields
    
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
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

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

    private void BindEvent()
    {
    }

    protected void ClosePopup()
    {
        //refactor
        Destroy(gameObject);
    }
    
    protected abstract void CreatePresenterByManager();

    #endregion
}