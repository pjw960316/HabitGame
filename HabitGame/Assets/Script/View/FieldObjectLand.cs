using System;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0414 // Field is assigned but its value is never used
#pragma warning disable CS0219 // Variable is assigned but its value is never used

public class FieldObjectLand : FieldObjectBase
{
    public enum EPath
    {
        Top,
        Bottom,
        Left,
        Right
    }
    public class PlacementHelper
    {
        private int _offset;
        private readonly Dictionary<EPath,Vector3> _placeVectorDictionary = new();
        
        public PlacementHelper(float environment_X_Length, float environment_Z_Length)
        {
            _placeVectorDictionary[EPath.Bottom] = new Vector3(environment_X_Length * _offset, 0, 0);
            _placeVectorDictionary[EPath.Top] = new Vector3(environment_X_Length * _offset, 0, environment_X_Length * SQUARE_SIDE_COUNT);
            _placeVectorDictionary[EPath.Left] = new Vector3(0, 0, environment_Z_Length * _offset);
            _placeVectorDictionary[EPath.Right] = new Vector3(environment_Z_Length * SQUARE_SIDE_COUNT, 0, environment_Z_Length * _offset);
        }

        public Vector3 GetPosition(EPath path, int offset)
        {
            _offset = offset;
            if (_placeVectorDictionary.TryGetValue(path, out var pos))
            {
                return pos;
            }

            throw new KeyNotFoundException();
        }
    }
    
    #region 1. Fields

    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private GameObject _rockPrefab;
    [SerializeField] private GameObject _mushroomPrefab;
    [SerializeField] private GameObject _bushPrefab;
    [SerializeField] private Transform _environmentsBaseTransform;

    private readonly int _borderEnvironmentsCount = 100;
    private const int SQUARE_SIDE_COUNT = 4;
    private int _oneSideEnvironmentCount;

    private readonly List<EPath> _createOrder = new()
    {
        EPath.Bottom,
        EPath.Top,
        EPath.Left,
        EPath.Right
    };

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void OnStart()
    {
        base.OnStart();

        CreateFieldObjectEnvironments();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();
        
        _oneSideEnvironmentCount = _borderEnvironmentsCount / SQUARE_SIDE_COUNT;
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.LAND;
    }

    protected sealed override void BindEvent()
    {
    }

    private void CreateFieldObjectEnvironments()
    {
        var list = new List<Transform>();
        for (var i = 0; i < _borderEnvironmentsCount; i++)
        {
            // note
            // high cost
            list.Add(Instantiate(_rockPrefab, _environmentsBaseTransform).transform);
        }

        var firstObject = list[0].GetComponent<FieldObjectEnvironmentBase>();
        var environment_X_Length = firstObject.GetEnvironment_X_Length();
        var environment_Z_Length = firstObject.GetEnvironment_Z_Length();

        var placementHelper = new PlacementHelper(environment_X_Length, environment_Z_Length);
        var offset = 0;

        for (var i = 0; i < SQUARE_SIDE_COUNT; i++)
        {
            for (var j = 0; j < _oneSideEnvironmentCount; j++)
            {
                var idx = i * j;
                list[idx].position = placementHelper.GetPosition(_createOrder[i], offset) +
                                     _environmentsBaseTransform.position;
                offset++;
            }

            offset = 0;
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