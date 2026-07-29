using UniRx;
using UnityEngine;

public abstract class FieldObjectBase : MonoBehaviour, IView
{
    #region 1. Fields
    
    protected PresenterManager _presenterManager;
    protected EFieldObject _eFieldObjectKey;
    protected int _instanceID;
    
    private Transform _fieldObjectTransform;
    
    private readonly Subject<Unit> _onDestroyFieldObject = new();

    #endregion

    #region 2. Properties

    public int InstanceID => _instanceID; 
    
    // TODO
    // 얘는 model이 관리하는 게 맞는 듯.
    public EFieldObject EFieldObjectKey => _eFieldObjectKey;

    public Subject<Unit> OnDestroyFieldObject => _onDestroyFieldObject;

    public Transform FieldObjectTransform => _fieldObjectTransform;

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        // TODO
        // 이거 model에서 하는 게 맞나?
        InitializeEnumFieldObjectKey();
        
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

    // TODO
    // 수정
    protected virtual void Initialize()
    {
        _presenterManager = PresenterManager.Instance;
        
        _instanceID = GetInstanceID();
        _fieldObjectTransform = transform;

        // NOTE
        // 반드시 EFieldObjectKey가 선행 세팅 되어야 한다.
        //_fieldObjectManager.RegisterFieldObjectInActiveDictionary(this);
    }

    protected abstract void InitializeEnumFieldObjectKey();
    protected abstract void CreatePresenterByManager();

    protected virtual void BindEvent()
    {
    }

    #endregion

    #region 4. EventHandlers

    // NOTE
    // virtual로 변경하지 마세요.
    private void OnDestroy()
    {
        _onDestroyFieldObject.OnNext(default);
    }

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
    PLAYER,
    SPARROW,
    LAND,
    ROCK,
    MUSHROOM,
    BUSH,
    FLOWER,
    GRASS,
    TREE,
    DEER
}
