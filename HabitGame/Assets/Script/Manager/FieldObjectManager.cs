using System.Collections.Generic;
using System.Linq;

public class FieldObjectManager : ManagerBase<FieldObjectManager>, IManager
{
    #region 1. Fields

    // note : key = InstanceID (UnityEngine.Object)
    private readonly Dictionary<int, FieldObjectBase> _activeFieldObjectDictionary = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
    }

    public void Initialize()
    {
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void SetModel(IEnumerable<IModel> models)
    {
    }

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

    #endregion
}