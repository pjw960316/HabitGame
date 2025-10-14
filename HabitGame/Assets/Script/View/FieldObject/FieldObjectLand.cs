using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldObjectLand : FieldObjectBase
{
    public enum EPath
    {
        Top,
        Bottom,
        Left,
        Right
    }

    #region 1. Fields

    private const int SQUARE_SIDE_COUNT = 4;
    private const int HORIZONTAL_COUNT = 8; // 가로는 고정 -> 카메라 각도
    
    [SerializeField] private GameObject _rockPrefab;
    [SerializeField] private Transform _environmentsBaseTransform;
    [SerializeField] private int _verticalCount;
    
    private int _rockBordersCount;

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

        _rockBordersCount = HORIZONTAL_COUNT * 2 + _verticalCount * 2;
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.LAND;
    }

    protected sealed override void BindEvent()
    {
        base.BindEvent();
    }

    private void CreateFieldObjectEnvironments()
    {
        var list = new List<Transform>();
        for (var i = 0; i < _rockBordersCount; i++)
        {
            // note
            // high cost
            list.Add(Instantiate(_rockPrefab, _environmentsBaseTransform).transform);
        }

        var firstObject = list[0].GetComponent<FieldObjectEnvironmentBase>();
        var environment_X_Length = firstObject.GetEnvironment_X_Length();
        var environment_Z_Length = firstObject.GetEnvironment_Z_Length();
        var environmentsBaseTransformPosition = _environmentsBaseTransform.position;
        var offset = 0;
        var idx = 0;

        // note
        // 0 = 아래 / 1 = 위 / 2 = 왼쪽 / 3 = 오른쪽
        for (var i = 0; i < SQUARE_SIDE_COUNT; i++)
        {
            var count = i < 2 ? HORIZONTAL_COUNT : _verticalCount;
            for (var j = 0; j < count; j++)
            {
                list[idx].position = GetPosition(_createOrder[i], offset, environment_X_Length, environment_Z_Length) +
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

    private Vector3 GetPosition(EPath path, int offset, float environment_X_Length, float environment_Z_Length)
    {
        return path switch
        {
            EPath.Bottom => new Vector3(environment_X_Length * offset, 0, 0),
            EPath.Top => new Vector3(environment_X_Length * offset, 0,
                environment_X_Length * _verticalCount),
            EPath.Left => new Vector3(0, 0, environment_Z_Length * offset),
            EPath.Right => new Vector3(environment_Z_Length * HORIZONTAL_COUNT, 0,
                environment_Z_Length * offset),
            _ => throw new NullReferenceException()
        };
    }

    #endregion


    protected override void CreatePresenterByManager()
    {
    }
}