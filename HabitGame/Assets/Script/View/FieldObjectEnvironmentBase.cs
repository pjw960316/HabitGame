using UnityEngine;

public abstract class FieldObjectEnvironmentBase : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] protected GameObject _lod_0_Environment_GameObject;

    protected Vector3 _lod_0_Environment_BoundSize;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        base.Initialize();

        _lod_0_Environment_BoundSize = _lod_0_Environment_GameObject.GetComponent<MeshRenderer>().bounds.size;
    }

    protected override void CreatePresenterByManager()
    {
        // no presenter
    }

    protected override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public float GetEnvironment_X_Length()
    {
        return _lod_0_Environment_BoundSize.x;
    }

    public float GetEnvironment_Z_Length()
    {
        return _lod_0_Environment_BoundSize.z;
    }

    #endregion
}