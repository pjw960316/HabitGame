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
        _alarmData = _modelManager.GetModel<AlarmData>();
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
            alarmAudioClipButton.OnButtonClicked.Subscribe(RequestUpdateLatestSleepingAudioClip).AddTo(_disposable);
        }

        foreach (var alarmTimeButton in _alarmPopup.AlarmTimeButtons)
        {
            alarmTimeButton.OnButtonClicked.Subscribe(RequestUpdateLatestSleepingAudioPlayTime).AddTo(_disposable);
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

        RequestPlaySleepingMusic();

        RequestOpenAlarmTimerPopup();

        Close();
    }

    #endregion

    #region 5. Request Methods

    private void RequestOpenAlarmTimerPopup()
    {
        var popupTargetTransform = _uiManager.MainCanvasTransform;

        _uiManager.OpenPopupByStringKey(EPopupKey.AlarmTimerPopup, popupTargetTransform);
    }

    private void RequestPlaySleepingMusic()
    {
        _soundManager.SetAudioSourceLoopOn();
        _soundManager.PlayMusic(_alarmData.LatestSleepingAudioClip);
    }

    private void RequestUpdateLatestSleepingAudioClip(EAlarmButtonType eAlarmAudioClip)
    {
        _alarmData.SetLatestSleepingAudioClip(eAlarmAudioClip);
    }

    private void RequestUpdateLatestSleepingAudioPlayTime(EAlarmButtonType eAlarmTime)
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

    #region 6. Methods

    //

    #endregion
}