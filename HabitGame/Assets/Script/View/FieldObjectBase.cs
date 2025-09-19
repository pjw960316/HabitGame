using UnityEngine;

public abstract class FieldObjectBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] protected Transform _myFieldObjectTransform; 
    
    protected EFieldObject _eFieldObjectKey;
    protected int _instanceID;

    protected PresenterManager _presenterManager;
    protected FieldObjectManager _fieldObjectManager;

    #endregion

    #region 2. Properties

    public Transform MyFieldObjectTransform => _myFieldObjectTransform;

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
        InitializeEnumFieldObjectKey();
        
        // note 
        // 반드시 EFieldObjectKey가 선행 세팅 되어야 한다.
        _fieldObjectManager.RegisterFieldObjectInActiveDictionary(this);
    }

    protected abstract void InitializeEnumFieldObjectKey();
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

public enum EFieldObject
{
    SPARROW,
    LAND,
    ROCK,
    MUSHROOM,
    BUSH,
    FLOWER,
    GRASS,
    TREE,
}