using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Random = System.Random;

// note : 
// fieldObjectManager가 fieldObject보다 먼저 생성된다.
public class FieldObjectManager : ManagerBase<FieldObjectManager>
{
    #region 1. Fields

    // note :
    // key = InstanceID (UnityEngine.Object)
    private readonly Dictionary<int, FieldObjectPresenterBase> _fieldObjectPresenterDict = new();
    private readonly Subject<Unit> _onUpdateTouchedFieldObject = new();
    private readonly Random _randomMaker = new();

    #endregion

    #region 2. Properties

    public Subject<Unit> OnUpdateTouchedFieldObject => _onUpdateTouchedFieldObject;

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        //
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void RegisterFieldObjectPresenter(FieldObjectPresenterBase fieldObjectPresenterBase)
    {
        var key = fieldObjectPresenterBase.GetFieldObjectInstanceID();
        
        if (!_fieldObjectPresenterDict.TryAdd(key, fieldObjectPresenterBase))
        {
            throw new InvalidOperationException("키가 고유인데 중복됩니다.");
        }
    }

    public void PrintFieldObjectPresenterDictionary()
    {
        foreach (var kv in _fieldObjectPresenterDict)
        {
            Debug.Log($"{kv.Key} , {kv.Value}");
        }
    }
    
    public FieldObjectSparrow GetRandomSparrow()
    {
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
            throw new ArgumentOutOfRangeException();
        }

        var randValue = _randomMaker.Next(0, sparrowCount);
        var tmpValue = 0;
        
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
/*
    public TFieldObject GetFieldObject<TFieldObject>(int instanceID) where TFieldObject : FieldObjectBase
    {
        _activeFieldObjectDictionary.TryGetValue(instanceID, out var fieldObjectBase);

        return fieldObjectBase as TFieldObject;
    }

    // note : 테스트 용도로 제작
    public FieldObjectSparrow GetFirstSparrow(int instanceID)
    {
        return _activeFieldObjectDictionary
            .Where(element => element.Value is FieldObjectSparrow)
            .FirstOrDefault(element => element.Key != instanceID).Value as FieldObjectSparrow;
    }

    public FieldObjectSparrow GetFirstSparrowAny()
    {
        return _activeFieldObjectDictionary
            .FirstOrDefault(element => element.Value is FieldObjectSparrow).Value as FieldObjectSparrow;
    }

    */

    #endregion
}