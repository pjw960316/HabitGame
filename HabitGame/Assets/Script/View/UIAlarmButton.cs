using UniRx;
using UnityEngine;

public class UIAlarmButton : UIButton
{
    //FIX
    //인터페이스로 들고 있으라는데 왜?
    private AlarmPresenter _alarmPresenter;
    private CompositeDisposable _disposable = new CompositeDisposable();

    protected override void Awake()
    {
        base.Awake();

        SoundManager.Instance.ConnectViewWithPresenter(this, _alarmPresenter);

        // TODO : UniRx를 이용해서 View 생성 시에 SoundManager에서 P에서 MVP들을 연결해보자. - 간접 호출 RX Pattern
    }

    protected override void BindEvent()
    {
        button?.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        //test
        //buttonText.text = _alarmPresenter.TestModelViewConnection().ToString();
        
        //test
        //test

        Debug.Log(SoundManager.Instance.GetHashCode());
        if (SoundManager.Instance == null)
        {
            Debug.Log("1");
            
        }

        SoundManager.Instance.TestEvent = new Subject<int>();
        if (SoundManager.Instance.TestEvent == null)
        {
            Debug.Log("2");
        }
        //test
        SoundManager.Instance.TestEvent.Subscribe(x => { Debug.Log(x.ToString()); }).AddTo(_disposable);
        
        SoundManager.Instance.TestEvent.OnNext(1);
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
}