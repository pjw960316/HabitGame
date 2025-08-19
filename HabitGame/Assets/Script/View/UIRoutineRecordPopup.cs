using UnityEngine;

public class UIRoutineRecordPopup : UIPopupBase
{
    #region 1. Fields

    //test
    //non - scroll
    [SerializeField] private UIWidgetBase _routineRecordWidgetTestOne;
    [SerializeField] private UIWidgetBase _routineRecordWidgetTestTwo;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void OnAwake()
    {
        base.OnAwake();

        CreatePresenterByManager();

        Initialize();
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
    }

    protected override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<RoutineRecordPresenter>(this);
    }

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}