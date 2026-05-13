using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Random = System.Random;

// Note
// fieldObjectManager가 fieldObject보다 먼저 생성된다.
public class FieldObjectManager : ManagerBase<FieldObjectManager>
{
    #region 1. Fields

    // Note
    // key = InstanceID (UnityEngine.Object)
    private readonly Dictionary<int, FieldObjectPresenterBase> _fieldObjectPresenterDict = new();
    private readonly Random _randomMaker = new();

    private PresenterManager _presenterManager;
    private InputManager _inputManager;
    
    #endregion

    #region 2. Properties
    //

    #endregion

    #region 3. Constructor

    // TODO
    // Manager는 어차피 계속 존재하니까 disposable이 필요한가?  
    public sealed override void Initialize()
    {
        _presenterManager = PresenterManager.Instance;
        _inputManager = InputManager.Instance;
        

        _inputManager.OnTouchedFieldObject
            .Subscribe(OnSelectedFieldObject);
        //.AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnSelectedFieldObject(FieldObjectBase targetFieldObject)
    {
        // FIX
        // 지금 gameLoad 씬에서도 이게 불림. 일단 막음
        if (targetFieldObject == null)
        {
            return;
        }

        var sparrowPresenter =
            _presenterManager.GetFieldObjectPresenter<FieldObjectSparrowPresenter>(targetFieldObject.InstanceID);
        
        sparrowPresenter.CommandChangeColor();
    }

    private void UpdateTouchedTarget(FieldObjectSparrowPresenter targetSparrowPresenter)
    {
        
        targetSparrowPresenter.CommandChangeColor(true);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // note
    // FieldObject가 생성되면 호출해서 Dictionary에 추가한다.
    // 의존성?
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