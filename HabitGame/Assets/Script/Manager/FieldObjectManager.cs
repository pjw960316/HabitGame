using System.Collections.Generic;

public class FieldObjectManager : ManagerBase<FieldObjectManager>, IManager
{
    #region 1. Fields

    private readonly Dictionary<EFieldObject, FieldObjectBase> _activeFieldObjectDictionary = new();

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
        var key = fieldObject.EFieldObjectKey;
        _activeFieldObjectDictionary[key] = fieldObject;
    }

    public TFieldObject GetFieldObject<TFieldObject>(EFieldObject eFieldObjectKey) where TFieldObject : FieldObjectBase
    {
        _activeFieldObjectDictionary.TryGetValue(eFieldObjectKey, out var fieldObjectBase);

        return fieldObjectBase as TFieldObject;
    }

    #endregion
}