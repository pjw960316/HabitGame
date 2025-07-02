using UnityEngine;

public class UIAlarmButton : UIButton
{
    #region 1. Fields
    
    private AlarmPresenter _alarmPresenter;
    private SoundManager _soundManager;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected override void Awake()
    {
        base.Awake();

        _soundManager = SoundManager.Instance;
        _soundManager.ConnectViewWithPresenter(this, _alarmPresenter);

        // TODO : UniRx를 이용해서 View 생성 시에 SoundManager에서 P에서 MVP들을 연결해보자. - 간접 호출 RX Pattern
    }

    #endregion

    #region 4. Methods

    // default
    protected override void BindEvent()
    {
        button?.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        //test
        _soundManager.TestEvent.OnNext(3);
    }

    //FIX
    //이 코드는 제거하거나 다른 곳에서 해야 한다.
    public void InjectPresenter(IPresenter presenter)
    {
        if (presenter is AlarmPresenter alarmPresenter)
        {
            _alarmPresenter = alarmPresenter;
        }
    }

    public sealed override void HoldPresenterInterface()
    {
    }

    #endregion
}