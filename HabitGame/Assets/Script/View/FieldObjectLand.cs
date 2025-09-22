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
        private readonly float _environment_X_Length;
        private readonly float _environment_Z_Length;
        private readonly int _oneSideEnvironmentCount;


        public PlacementHelper(float environment_X_Length, float environment_Z_Length, int oneSideEnvironmentCount)
        {
            _environment_X_Length = environment_X_Length;
            _environment_Z_Length = environment_Z_Length;
            _oneSideEnvironmentCount = oneSideEnvironmentCount;
        }

        public Vector3 GetPosition(EPath path, int offset)
        {
            return path switch
            {
                EPath.Bottom => new Vector3(_environment_X_Length * offset, 0, 0),
                EPath.Top => new Vector3(_environment_X_Length * offset, 0, _environment_X_Length * _oneSideEnvironmentCount),
                EPath.Left => new Vector3(0, 0, _environment_Z_Length * offset),
                EPath.Right => new Vector3(_environment_Z_Length * _oneSideEnvironmentCount, 0,
                    _environment_Z_Length * offset),
                _ => throw new NullReferenceException()
            };
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

        var placementHelper = new PlacementHelper(environment_X_Length, environment_Z_Length, _oneSideEnvironmentCount);
        var environmentsBaseTransformPosition = _environmentsBaseTransform.position;
        var offset = 0;
        var idx = 0;

        for (var i = 0; i < SQUARE_SIDE_COUNT; i++)
        {
            for (var j = 0; j < _oneSideEnvironmentCount; j++)
            {
                list[idx].position = placementHelper.GetPosition(_createOrder[i], offset) +
                                     environmentsBaseTransformPosition;
                idx++;
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