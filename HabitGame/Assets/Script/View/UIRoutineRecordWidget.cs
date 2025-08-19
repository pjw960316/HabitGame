using UnityEngine;

public class UIRoutineRecordWidget : UIWidgetBase
{
    #region 1. Fields

    // default

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void OnAwake()
    {
        base.OnAwake();

        Initialize();
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
        //test
        Debug.Log("widget Intialize");
    }

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}