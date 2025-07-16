using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBase<GameManager>, IManager
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
        TurnOnScreenAlways();
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<ScriptableObject> _list)
    {
    }

    public void ConnectInstanceByActivator(IManager instance)
    {
    }

    private void TurnOnScreenAlways()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}