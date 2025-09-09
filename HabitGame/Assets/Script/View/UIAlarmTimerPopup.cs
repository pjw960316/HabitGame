using System;
using UniRx;
using UnityEngine;

public class UIAlarmTimerPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private UIImageBase _titleWidget;
    [SerializeField] private UIImageBase _alarmTimerWidget;
    [SerializeField] private UIButtonBase _quitAlarmButton;

    private readonly Subject<Unit> _onQuitAlarm = new();

    #endregion

    #region 2. Properties

    public IObservable<Unit> OnQuitAlarm => _onQuitAlarm;

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();

        InitializeEPopupKey();
    }

    // note
    // 얘는 연결만 된다.
    protected override void CreatePresenterByManager()
    {
        //
    }

    protected override void InitializeEPopupKey()
    {
        _ePopupKey = EPopupKey.AlarmTimerPopup;
    }

    protected sealed override void BindEvent()
    {
        _quitAlarmButton.OnClick.AddListener(OnClickQuitAlarmButton);
    }

    #endregion

    #region 4. EventHandlers

    private void OnClickQuitAlarmButton()
    {
        _onQuitAlarm.OnNext(default);
        _uiToastManager.ShowToast(EToastStringKey.EAlarmQuit);
        
        ClosePopup();
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void SetAlarmHeaderText(string text)
    {
        _titleWidget.SetText(text);
    }

    public void UpdateAlarmTimerText(string text)
    {
        var timeText = StringManager.Instance.GetUIString(EStringKey.EAlarmTimerPopupTime, text);
        _alarmTimerWidget.SetText(timeText);
    }

    #endregion
}