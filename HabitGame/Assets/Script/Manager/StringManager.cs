using System;
using System.Collections.Generic;
using UnityEngine;

public class StringManager : ManagerBase<StringManager>, IManager, IDisposable
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    public StringData StringData { get; private set; }

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<ScriptableObject> models)
    {
        foreach (var model in models)
        {
            if (model is StringData stringData)
            {
                StringData = stringData;
                return;
            }
        }
    }

    public string GetUIString(EStringKey eStringKey)
    {
        ExceptionHelper.CheckNullException("StringData", StringData);

        return StringData.GetStringByEStringKey(eStringKey);
    }

    public void Dispose()
    {
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}