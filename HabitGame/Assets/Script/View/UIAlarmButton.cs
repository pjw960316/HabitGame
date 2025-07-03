using System;
using UniRx;
using UnityEngine;

public class UIAlarmButton : UIButton
{
    #region 1. Fields

    private UIManager _uiManager;
    private SoundManager _soundManager;
    private IPresenter _alarmPresenter;

    //test
    private readonly Subject<Unit> _testEvent = new Subject<Unit>();
    public IObservable<Unit> TestEventAsObservable => _testEvent;
    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected override void Awake()
    {
        base.Awake();

        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;
        _alarmPresenter = _soundManager.GetPresenterAfterCreate(this);
    }

    #endregion

    #region 4. Methods
    
    protected override void BindEvent()
    {
        button?.onClick.AddListener(OnClicked);
    }
    
    #endregion
    
    #region 5. EventHandlers
    protected sealed override void OnClicked()
    {
        _testEvent.OnNext(Unit.Default);
    }
    
    #endregion
}