using System;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : ManagerBase<ScriptableObjectManager>
{
    #region 1. Fields

    private List<ScriptableObject> _scriptableObjectList = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    //

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void RegisterAllScriptableObjects(List<ScriptableObject> scriptableObjectList)
    {
        _scriptableObjectList.Clear();
        _scriptableObjectList = scriptableObjectList;
    }

    public T GetScriptableObject<T>() where T : ScriptableObject
    {
        foreach (var scriptableObject in _scriptableObjectList)
        {
            if (scriptableObject is T targetScriptableObject)
            {
                return targetScriptableObject;
            }
        }

        throw new NullReferenceException("GetModel Fail");
    }

    #endregion
}