using System;
using UniRx;

//TODO 
//얘는 사실 별 기능 없음.
//대부분 기능 제거해야함. MVP 없는 간단한 구조임
//UIButton 수준의 기능만 필요하다.
public class UIAlarmButton : UIButton
{
    #region 1. Fields
    
    private IPresenter _alarmPresenter;

    //test
    private readonly Subject<Unit> _testEvent = new();
    public IObservable<Unit> TestEventAsObservable => _testEvent;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected override void Awake()
    {
        base.Awake();

    }

    #endregion

    #region 4. Methods

    protected override void BindEvent()
    {
        Button?.onClick.AddListener(OnClicked);
    }

    #endregion

    #region 5. EventHandlers

    protected sealed override void OnClicked()
    {
        //_testEvent.OnNext(Unit.Default);
    }

    #endregion
}