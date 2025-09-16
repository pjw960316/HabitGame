using UnityEngine;

public abstract class FieldObjectBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] protected Transform FieldObjectTransform;
    [SerializeField] protected Animator FieldObjectAnimator;

    protected PresenterManager _presenterManager;
    
    #endregion

    #region 2. Properties

    //

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
        _presenterManager = PresenterManager.Instance;
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

    // 

    #endregion
}