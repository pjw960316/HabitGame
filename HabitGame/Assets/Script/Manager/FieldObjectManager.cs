using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Random = System.Random;

// NOTE
// fieldObjectManager가 fieldObject보다 먼저 생성된다.
// Manager들은 Unity 세계에서 View를 알기 때문에 View를 통해 Presenter를 얻어와야 한다.
public class FieldObjectManager : ManagerBase<FieldObjectManager>
{
    #region 1. Fields

    // NOTE
    // (Key , Value)  =>  (View의 InstanceID , FieldObjectPresenterBase)
    private readonly Dictionary<int, FieldObjectPresenterBase> _fieldObjectPresenterDict = new(); 
    
    private readonly Random _randomMaker = new();
    private InputManager _inputManager;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    // TODOㅑ
    // Manager는 어차피 계속 존재하니까 disposable이 필요한가?  
    public sealed override void Initialize()
    {
        _inputManager = InputManager.Instance;

        _inputManager.OnTouchedFieldObject
            .Subscribe(OnSelectedFieldObject);
        //.AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnSelectedFieldObject(FieldObjectBase targetFieldObject)
    {
        if (targetFieldObject == null)
        {
            return;
        }

        var targetInstanceID = targetFieldObject.InstanceID;

        foreach (var kv in _fieldObjectPresenterDict)
        {
            if (kv.Value is FieldObjectSparrowPresenter sparrowPresenter)
            {
                sparrowPresenter.UpdateTarget(kv.Key == targetInstanceID);
            }
        }
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // NOTE
    // FieldObject가 생성되면 호출해서 Dictionary에 추가한다.
    public void RegisterFieldObjectPresenter(FieldObjectPresenterBase fieldObjectPresenterBase)
    {
        var instanceID = fieldObjectPresenterBase.GetFieldObjectInstanceID();

        if (!_fieldObjectPresenterDict.TryAdd(instanceID, fieldObjectPresenterBase))
        {
            throw new InvalidOperationException("키가 고유인데 중복됩니다.");
        }
    }


    public FieldObjectSparrow GetRandomSparrow()
    {
        const int EXPECTED_SPARROW_COUNT = 8;

        var sparrowCount = 0;

        foreach (var kv in _fieldObjectPresenterDict)
        {
            var fieldObjectPresenter = kv.Value;

            if (fieldObjectPresenter is FieldObjectSparrowPresenter)
            {
                sparrowCount++;
            }
        }

        if (sparrowCount == 0)
        {
            Debug.LogError(
                $"[GetRandomSparrow][FAIL] No SparrowPresenter is registered. " +
                $"ExpectedSparrowCount: {EXPECTED_SPARROW_COUNT}, " +
                $"RegisteredPresenterCount: {_fieldObjectPresenterDict.Count}");

            throw new ArgumentOutOfRangeException();
        }

        var randValue = _randomMaker.Next(0, sparrowCount);
        var tmpValue = 0;

        var isExpectedSparrowCount = sparrowCount == EXPECTED_SPARROW_COUNT;
        var isRandomValueInRange = randValue >= 0 && randValue < sparrowCount;

        if (isExpectedSparrowCount && isRandomValueInRange)
        {
            Debug.Log(
                $"[GetRandomSparrow][PASS] SparrowCount: {sparrowCount}, " +
                $"ValidRandomRange: [0, {sparrowCount}), RandomValue: {randValue}");
        }
        else
        {
            var registeredSparrowInstanceIDs = _fieldObjectPresenterDict
                .Where(kv => kv.Value is FieldObjectSparrowPresenter)
                .Select(kv => kv.Key);

            Debug.LogError(
                $"[GetRandomSparrow][FAIL] ExpectedSparrowCount: {EXPECTED_SPARROW_COUNT}, " +
                $"ActualSparrowCount: {sparrowCount}, " +
                $"ValidRandomRange: [0, {sparrowCount}), RandomValue: {randValue}, " +
                $"RegisteredPresenterCount: {_fieldObjectPresenterDict.Count}, " +
                $"SparrowInstanceIDs: [{string.Join(", ", registeredSparrowInstanceIDs)}]");
        }

        var sparrowPresenters = _fieldObjectPresenterDict
            .Select(kv => kv.Value)
            .OfType<FieldObjectSparrowPresenter>();

        foreach (var sparrowPresenter in sparrowPresenters)
        {
            if (tmpValue == randValue)
            {
                return sparrowPresenter.GetFieldObjectSparrow();
            }

            tmpValue++;
        }

        return null;
    }

    public Transform GetPlayerTransform()
    {
        var playerPresenter = _fieldObjectPresenterDict
            .Select(kv => kv.Value)
            .OfType<FieldObjectPlayerPresenter>()
            .FirstOrDefault();

        if (playerPresenter == null)
        {
            throw new InvalidOperationException("PlayerPresenter is not registered.");
        }

        return playerPresenter.GetFieldObjectTransform();
    }

    public TFieldObjectPresenter GetFieldObjectPresenter<TFieldObjectPresenter>(int instanceID) where TFieldObjectPresenter : FieldObjectPresenterBase
    {
        if (!_fieldObjectPresenterDict.TryGetValue(instanceID, out var presenter))
        {
            throw new KeyNotFoundException(
                $"FieldObjectPresenter not found. InstanceID: {instanceID}");
        }

        if (presenter is not TFieldObjectPresenter typedPresenter)
        {
            throw new InvalidCastException(
                $"Invalid presenter type. InstanceID: {instanceID}, " +
                $"Expected: {typeof(TFieldObjectPresenter).Name}, Actual: {presenter.GetType().Name}");
        }

        return typedPresenter;
    }

    #endregion
}
