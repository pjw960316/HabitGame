using UnityEngine;

public class UIAlarmButton : UIButton
{
    private AlarmPresenter  _alarmPresenter;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void BindEvent()
    {
        button?.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        
    }
}