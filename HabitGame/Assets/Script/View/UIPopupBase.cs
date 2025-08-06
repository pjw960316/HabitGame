using UnityEngine;

// note
// popup에서 Toast를 출력할 책임을 부여해야 하는가?
// 일단은 했다.
public abstract class UIPopupBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] protected UIToastBase _uiToastMessage;
    
    protected UIManager _uiManager;
    protected SoundManager _soundManager;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
    }

    protected abstract void CreatePresenterByManager();

    protected UIToastBase GetUIToast()
    {
        ExceptionHelper.CheckNullException(_uiToastMessage, "_uiToastMessage");

        return _uiToastMessage;
    }
    
    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}