using UnityEngine;

public class UIAlarmTimerPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private UIImageBase _titleWidget;
    [SerializeField] private UIImageBase _alarmTimerWidget;
    [SerializeField] private UIButtonBase _quitAlarmButton;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }
    protected sealed override void Initialize()
    {
        base.Initialize();
    }

    // note
    // 얘는 연결만 된다.
    protected override void CreatePresenterByManager()
    {
        //
    }
    
    protected sealed override void BindEvent()
    {
        
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void SetAlarmHeaderText(string text)
    {
        
    }
    public void UpdateAlarmTimerText(string text)
    {
        var timeText = StringManager.Instance.GetUIString(EStringKey.EAlarmTimerPopupTime, text);
        _alarmTimerWidget.SetText(timeText);
    }

    #endregion

    
}
