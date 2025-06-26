using UnityEngine;

public class UIAlarmButton : UIButton
{
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