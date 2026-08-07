using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldObjectLand : FieldObjectBase
{
    private enum EPath
    {
        Top,
        Bottom,
        Left,
        Right
    }

    #region 1. Fields

    // NOTE
    // 맵 데이터 필드 3개
    private const int BORDER_SIDE_COUNT = 4;
    [SerializeField] private int _horizontalRockCount;
    [SerializeField] private int _verticalRockCount;
    
    [SerializeField] private GameObject _rockPrefab;
    [SerializeField] private Transform _environmentsBaseTransform;
    
    
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

        _rockBordersCount = _horizontalRockCount * 2 + _verticalRockCount * 2;
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.LAND;
    }

    protected sealed override void BindEvent()
    {
        base.BindEvent();
    }

    // NOTE
    // 일단은 풀링을 고려하지 않음.
    // Presenter가 해야 하냐에 대한 답은 아직 모호하지만 이건 View에 있어도 무방하다고 판단하다.
    private void CreateFieldObjectEnvironments()
    {
        var list = new List<Transform>();
        for (var i = 0; i < _rockBordersCount; i++)
        {
            // NOTE
            // high cost
            list.Add(Instantiate(_rockPrefab, _environmentsBaseTransform).transform);
        }

        var firstObject = list[0].GetComponent<FieldObjectEnvironmentBase>();
        var environment_X_Length = firstObject.GetEnvironment_X_Length();
        var environment_Z_Length = firstObject.GetEnvironment_Z_Length();
        var environmentsBaseTransformPosition = _environmentsBaseTransform.position;
        var offset = 0;
        var idx = 0;

        // NOTE
        // 0 = 아래 / 1 = 위 / 2 = 왼쪽 / 3 = 오른쪽
        for (var i = 0; i < BORDER_SIDE_COUNT; i++)
        {
            var count = i < 2 ? _horizontalRockCount : _verticalRockCount;
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

    #region 5. Methods

    private Vector3 GetPosition(EPath path, int offset, float environment_X_Length, float environment_Z_Length)
    {
        return path switch
        {
            EPath.Bottom => new Vector3(environment_X_Length * offset, 0, 0),
            EPath.Top => new Vector3(environment_X_Length * offset, 0,
                environment_X_Length * _verticalRockCount),
            EPath.Left => new Vector3(0, 0, environment_Z_Length * offset),
            EPath.Right => new Vector3(environment_Z_Length * _horizontalRockCount, 0,
                environment_Z_Length * offset),
            _ => throw new NullReferenceException()
        };
    }

    #endregion


    protected override void CreatePresenterByManager()
    {
    }
}
