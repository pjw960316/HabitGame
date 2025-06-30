using UnityEngine;

public class UIAlarmButton : UIButton
{
    //TODO : 인터페이스로 들고 있으라는데 왜?
    private AlarmPresenter _alarmPresenter;
    
    protected override void Awake()
    {
        base.Awake();
        
        Debug.Log("Test");
        
        SoundManager.Instance.ConnectViewWithPresenter(this, _alarmPresenter);
    }

    protected override void BindEvent()
    {
        button?.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        buttonText.text = _alarmPresenter.TestModelViewConnection().ToString();
    }

    //TODO : 일단
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