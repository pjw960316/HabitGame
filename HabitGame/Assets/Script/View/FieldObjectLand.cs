using UnityEngine;

public class FieldObjectLand : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private GameObject _rockPrefab;
    [SerializeField] private GameObject _mushroomPrefab;
    [SerializeField] private GameObject _bushPrefab;
    [SerializeField] private Transform _environmentsBaseTransform;
        
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
        CreateFieldObjectEnvironments();
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

    private void CreateFieldObjectEnvironments()
    {
        //test
        var rock = Instantiate(_rockPrefab, _environmentsBaseTransform);
        
        var renderer = rock.GetComponent<Renderer>();
        Vector3 size = renderer.bounds.size; // x=가로, y=세로, z=깊이
        Debug.Log($"Width: {size.x}, Height: {size.y}, Depth: {size.z}");
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