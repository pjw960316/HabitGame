using System.Collections.Generic;
using System.Collections.Immutable;
using TMPro;
using UnityEngine;

public class UIRoutineRecordWidget : UIWidgetBase
{
    #region 1. Fields

    [SerializeField] private UIImageBase _dateWidget;
    [SerializeField] private List<UIImageBase> _routineRecordWidget;
    [SerializeField] private UIImageBase _routineRecordMoneyWidget;
    

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
        //
    }

    public void SetData(KeyValuePair<string, ImmutableList<bool>> routineRecordElement)
    {
        _dateWidget.SetText(routineRecordElement.Key);
    }
    

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}