using System;
using System.Collections.Generic;
using UnityEngine;

public class StringManager : ManagerBase<StringManager>, IManager, IDisposable
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    private StringData _stringData;

    public StringData StringData
    {
        get
        {
            if (_stringData != null)
            {
                return _stringData;
            }

            throw new NullReferenceException("_stringData is Null");
        }
    }

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<IModel> models)
    {
        foreach (var model in models)
        {
            if (model is StringData stringData)
            {
                _stringData = stringData;
                return;
            }
        }
    }

    public string GetUIString(EStringKey eStringKey)
    {
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