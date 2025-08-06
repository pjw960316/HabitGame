using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    #region 1. Fields

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


    public abstract void CreatePresenterByManager();

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}