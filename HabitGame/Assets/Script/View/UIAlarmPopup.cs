using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

// fix
// 일단 UIManager에서 만들지 않고, 그냥 GameObject로 올려놓고 테스트
public class UIAlarmPopup : UIPopupBase
{
    #region 1. Fields

    //test
    private UIAlarmButton _view;
    private UIManager _uiManager;
    private SoundManager _soundManager;
    private IPresenter _alarmPresenter;
    [SerializeField] private Button _musicButton_1;

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

        _alarmPresenter = _soundManager.GetPresenterAfterCreate(this);

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _musicButton_1.onClick.AddListener(Onclicked);
    }

    #endregion

    #region 5. EventHandlers

    private void Onclicked()
    {
        _onSoundButtonClicked.OnNext(Unit.Default);
    }

    #endregion
}