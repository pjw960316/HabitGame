using UnityEngine;

public class FieldObjectLand : FieldObjectBase
{
    #region 1. Fields

    private FieldObjectSparrow _fieldObjectSparrow;
    private Transform _sparrowTransform;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();

        SetFieldObjectSparrow();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();

        // test code start
    }

    protected sealed override void InitializeEnumKey()
    {
        _eFieldObjectKey = EFieldObject.LAND;
    }

    protected sealed override void BindEvent()
    {
    }

    private void SetFieldObjectSparrow()
    {
        _fieldObjectSparrow = _fieldObjectManager.GetFieldObject<FieldObjectSparrow>(EFieldObject.SPARROW);
        _sparrowTransform = _fieldObjectSparrow.MyFieldObjectTransform;
    }

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


    protected override void CreatePresenterByManager()
    {
    }
}