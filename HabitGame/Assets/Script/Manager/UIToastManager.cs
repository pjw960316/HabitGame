using System;
using System.Collections.Generic;
using UnityEngine;

public class UIToastManager : ManagerBase<UIToastManager>, IManager, IDisposable
{
    #region 1. Fields

    // default

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<ScriptableObject> _list)
    {
    }
    
    #endregion

    #region 5. EventHandlers

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    #endregion
}