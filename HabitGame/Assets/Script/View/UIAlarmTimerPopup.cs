using UnityEngine;

public class UIAlarmTimerPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private UIImageBase _alarmTimerWidget;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    //

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void UpdateAlarmTimerText(string text)
    {
        var timeText = StringManager.Instance.GetUIString(EStringKey.EAlarmTimerPopupTime, text);
        _alarmTimerWidget.SetText(timeText);
    }

    #endregion

    // note
    // 얘는 연결만 된다.
    protected override void CreatePresenterByManager()
    {
        //
    }
}
