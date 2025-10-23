using UnityEngine;

public class GameManager : ManagerBase<GameManager>
{
    #region 1. Fields

    // default

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        TurnOnScreenAlways();
    }

    #endregion

    #region 4. Methods

    private void TurnOnScreenAlways()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}