using UniRx;
using UnityEngine;

public class UIAlarmButton : UIButton
{
    #region 1. Fields

    private SoundManager _soundManager;
    private IPresenter _alarmPresenter;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected override void Awake()
    {
        base.Awake();

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
        _soundManager.TestEvent.OnNext(Unit.Default);
    }
    
    #endregion
}