using System;
using System.Collections.Generic;

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

    public void PreInitialize()
    {
    }

    public void Initialize()
    {
    }
    
    public void LateInitialize()
    {
        //
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

    public string GetUIString(EStringKey eStringKey, params object[] args)
    {
        if (args.Length == 0)
        {
            return StringData.GetStringByEStringKey(eStringKey);
        }

        return StringData.GetStringByEStringKeyAndArguments(eStringKey, args);
    }

    public string GetToastString(EToastStringKey eToastStringKey)
    {
        return StringData.GetToastString(eToastStringKey);
    }

    public void Dispose()
    {
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}