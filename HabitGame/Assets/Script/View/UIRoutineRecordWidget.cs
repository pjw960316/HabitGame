using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;

public class UIRoutineRecordWidget : UIWidgetBase
{
    #region 1. Fields

    [SerializeField] private UIImageBase _dateWidget;
    [SerializeField] private List<UIImageBase> _routineRecordWidget;
    [SerializeField] private UIImageBase _routineRecordMoneyWidget;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;

    private MyCharacterManager _myCharacterManager;

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
        _myCharacterManager = MyCharacterManager.Instance;
    }

    //test code
    public void SetData(KeyValuePair<string, ImmutableList<bool>> routineRecordElement)
    {
        _dateWidget.SetText(routineRecordElement.Key);

        var test = routineRecordElement.Value;
        var successCount = 0;

        for (var index = 0; index < test.Count; index++)
        {
            if (test[index])
            {
                _routineRecordWidget[index].SetColor(Color.green);
                successCount++;
            }
            else
            {
                _routineRecordWidget[index].SetColor(Color.red);
            }
        }

        var moneyText = (_myCharacterManager.GetMoneyPerRoutineSuccess() * successCount).ToString();
        _routineRecordMoneyWidget.SetText(moneyText);
    }

    public void UpdateData()
    {
    }

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}