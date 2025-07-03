using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase<UIManager>, IManager
{
    #region 1. Fields
    // default
    #endregion
    
    #region 2. Properties
    // default
    #endregion
    
    #region 3. Constructor
    // default
    #endregion

    #region 4. Methods
    public void Init()
    {
    }

    public void SetModel(IEnumerable<ScriptableObject> _list)
    {
    }

    public void ConnectInstanceByActivator(IManager instance)
    {
    }
    
    // TODO 
    // 비동기를 구현하려면 여기서.
    public void OpenPopup()
    {}
    
    #endregion
    
    #region 5. EventHandlers
    // default
    #endregion
    
}