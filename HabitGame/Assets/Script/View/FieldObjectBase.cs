using System;
using UnityEngine;

public abstract class FieldObjectBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] protected Transform _myFieldObjectTransform;

    protected int _instanceID;
    protected EFieldObject _eFieldObjectKey;
    
    protected PresenterManager _presenterManager;
    protected FieldObjectManager _fieldObjectManager;
    
    #endregion

    #region 2. Properties

    public EFieldObject EFieldObjectKey => _eFieldObjectKey;

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

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        
    }

    protected virtual void Initialize()
    {
        _presenterManager = PresenterManager.Instance;
        _fieldObjectManager = FieldObjectManager.Instance;

        _instanceID = GetInstanceID();
        InitializeEnumKey();
    }

    protected abstract void BindEvent();
    protected abstract void CreatePresenterByManager();
    protected abstract void InitializeEnumKey();

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

public enum EFieldObject
{
    SPARROW,
    LAND,
}