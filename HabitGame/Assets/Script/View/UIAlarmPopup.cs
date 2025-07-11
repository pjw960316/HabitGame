using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIAlarmPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private List<Button> _alarmMusicButtons = new();
    [SerializeField] private List<Button> _timeButtons = new();

    private UIManager _uiManager;
    private SoundManager _soundManager;
    private AlarmPresenter _alarmPresenter;

    private readonly Subject<Unit> _onSoundButtonClicked = new();
    public IObservable<Unit> OnSoundButtonClicked => _onSoundButtonClicked;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected void Awake()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
    }

    #endregion

    #region 5. EventHandlers

    private void OnClicked()
    {
        _onSoundButtonClicked.OnNext(Unit.Default);
    }

    #endregion
}