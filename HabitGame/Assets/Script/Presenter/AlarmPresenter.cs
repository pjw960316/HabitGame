using UniRx;

public class AlarmPresenter : UIPresenterBase
{
    #region 1. Fields

    private UIAlarmPopup _alarmPopup;
    private AlarmData _alarmData;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);
    }

    protected sealed override void InitializeView()
    {
        base.InitializeView();

        _alarmPopup = _view as UIAlarmPopup;
        ExceptionHelper.CheckNullException(_alarmPopup, "_alarmPopup");
    }

    protected sealed override void InitializeModel()
    {
        _alarmData = _scriptableObjectManager.GetScriptableObject<AlarmData>();
        ExceptionHelper.CheckNullException(_alarmData, "_alarmData");
    }

    public sealed override void SetView()
    {
        _alarmPopup.SetButtonText(_alarmData.SleepingAudioPlayTimeDictionary);
    }

    public sealed override void BindEvent()
    {
        foreach (var alarmAudioClipButton in _alarmPopup.AlarmAudioClipButtons)
        {
            alarmAudioClipButton.OnButtonClicked.Subscribe(UpdateLatestSleepingAudioClip).AddTo(_disposable);
        }

        foreach (var alarmTimeButton in _alarmPopup.AlarmTimeButtons)
        {
            alarmTimeButton.OnButtonClicked.Subscribe(UpdateLatestSleepingAudioPlayTime).AddTo(_disposable);
        }

        _alarmPopup.OnConfirmed.Subscribe(_ => OnStartAlarmSystem()).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnStartAlarmSystem()
    {
        if (!IsAlarmDataSelected())
        {
            _uiToastManager.ShowToast(EToastStringKey.EAlarmSelectPlease);

            return;
        }

        PlaySleepingMusic();

        OpenAlarmTimerPopup();

        Close();
    }

    #endregion

    #region 5. Methods

    private void OpenAlarmTimerPopup()
    {
        var popupTargetTransform = _uiManager.MainCanvasTransform;

        _uiManager.OpenPopupByStringKey(EPopupKey.AlarmTimerPopup, popupTargetTransform);
    }

    private void PlaySleepingMusic()
    {
        _soundManager.SetAudioSourceLoopOn();
        _soundManager.PlayMusic(_alarmData.LatestSleepingAudioClip);
    }

    private void UpdateLatestSleepingAudioClip(EAlarmButtonType eAlarmAudioClip)
    {
        _alarmData.SetLatestSleepingAudioClip(eAlarmAudioClip);
    }

    private void UpdateLatestSleepingAudioPlayTime(EAlarmButtonType eAlarmTime)
    {
        _alarmData.SetLatestSleepingAudioPlayTime(eAlarmTime);
    }

    private bool IsAlarmDataSelected()
    {
        if (_alarmData.LatestSleepingAudioPlayTime == 0f)
        {
            return false;
        }

        return true;
    }

    #endregion
}
