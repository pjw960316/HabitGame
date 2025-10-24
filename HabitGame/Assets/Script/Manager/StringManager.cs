using System;
using System.Collections.Generic;

public class StringManager : ManagerBase<StringManager>
{
    #region 1. Fields

    private StringData _stringData;

    #endregion

    #region 2. Properties

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

    public sealed override void SetModel()
    {
        _stringData = ScriptableObjectManager.Instance.GetScriptableObject<StringData>();
        ExceptionHelper.CheckNullException(_stringData, "_stringData");
    }

    #endregion

    #region 4. Methods

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