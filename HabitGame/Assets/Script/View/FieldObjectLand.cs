using System.Collections.Generic;
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

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void OnStart()
    {
        base.OnStart();

        SetFieldObjectSparrow();
        CreateFieldObjectEnvironments();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.LAND;
    }

    protected sealed override void BindEvent()
    {
    }

    private void SetFieldObjectSparrow()
    {
        _fieldObjectSparrow = _fieldObjectManager.GetFieldObject<FieldObjectSparrow>(EFieldObject.SPARROW);
    }

    private void CreateFieldObjectEnvironments()
    {
        // refactor
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 100; i++)
        {
            list.Add(Instantiate(_rockPrefab, _environmentsBaseTransform));
        }
        
        var x = list[0].GetComponent<FieldObjectEnvironmentBase>().GetEnvironment_X_Length();
        var z = list[0].GetComponent<FieldObjectEnvironmentBase>().GetEnvironment_Z_Length();
        

        int idx = 0;
        for (int i = 0; i < 25; i++)
        {
            list[i].transform.position = new Vector3(x * idx, 0, 0) + _environmentsBaseTransform.position;
            idx++;
        }

        idx = 0;
        for (int i = 25; i < 50; i++)
        {
            list[i].transform.position = new Vector3(x * idx, 0, x * 25) + _environmentsBaseTransform.position;
            idx++;
        }

        idx = 0;
        for (int i = 50; i < 75; i++)
        {
            list[i].transform.position = new Vector3(0, 0, z * idx) + _environmentsBaseTransform.position;
            idx++;
        }

        idx = 0;
        for (int i = 75; i < 100; i++)
        {
            list[i].transform.position = new Vector3(z * 25, 0, z * idx) + _environmentsBaseTransform.position;
            idx++;
        }
       
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