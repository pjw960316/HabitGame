using UnityEngine;

public class FieldObjectSparrow : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] protected Animator _sparrowAnimator;
    
    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();
    }

    protected sealed override void InitializeEnumKey()
    {
        _eFieldObjectKey = EFieldObject.SPARROW;
    }

    protected sealed override void BindEvent()
    {
        //
    }

    #endregion

    #region 4. EventHandlers

    private void FixedUpdate()
    {
        //
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected sealed override void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter2<SparrowPresenter>(this);
    }

    #endregion
}