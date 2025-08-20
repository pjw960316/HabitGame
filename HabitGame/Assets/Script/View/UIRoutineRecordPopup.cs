using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

public class UIRoutineRecordPopup : UIPopupBase
{
    #region 1. Fields

    //test
    //non - scroll
    [SerializeField] private UIRoutineRecordWidget _routineRecordWidgetTestOne;
    [SerializeField] private UIRoutineRecordWidget _routineRecordWidgetTestTwo;

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

    //test
    //non-scroll
    public void UpdateRoutineRecord(ImmutableDictionary<string, ImmutableList<bool>> routineRecordDictionary)
    {
        var routineRecordReverseDictionary = routineRecordDictionary.Reverse();

        int count = 0;
        foreach (var i in routineRecordReverseDictionary)
        {
            if (count == 0)
            {
                _routineRecordWidgetTestOne.SetData(i);
            }
            else
            {
                _routineRecordWidgetTestTwo.SetData(i);
            }
            count++;
        }
    }

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}