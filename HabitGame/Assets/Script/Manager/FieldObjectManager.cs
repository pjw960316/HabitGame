using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

// note : 
// fieldObjectManager가 fieldObject보다 먼저 생성된다.
public class FieldObjectManager : ManagerBase<FieldObjectManager>
{
    #region 1. Fields

    // note : key = InstanceID (UnityEngine.Object)
    // todo : manager가 view를 들고 있다... -> presenter 들고 있게 수정하자.
    private readonly Dictionary<int, FieldObjectBase> _activeFieldObjectDictionary = new();
    
    private readonly Subject<Unit> _onUpdateTouchedFieldObject = new();

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

    public void RegisterFieldObjectInActiveDictionary(FieldObjectBase fieldObject)
    {
        var key = fieldObject.InstanceID;
        _activeFieldObjectDictionary[key] = fieldObject;
    }

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

    public FieldObjectSparrow GetRandomSparrow()
    {
        var aliveSparrowContainer =
            _activeFieldObjectDictionary.Where(element => element.Value is FieldObjectSparrow).ToList();

        if (aliveSparrowContainer.Count == 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        var rand = new Random();
        var randValue = rand.Next(0, aliveSparrowContainer.Count - 1);

        return aliveSparrowContainer[randValue].Value as FieldObjectSparrow;
    }

    #endregion
}