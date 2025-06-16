using UnityEngine;

public class AlarmPresenter : IPresenter
{
    private UIAlarmButton _alarmButton;
    private SoundData _soundData;

    private static AudioClip _test;
    public void BindData()
    {
        throw new System.NotImplementedException();
    }

    private void Test()
    {
        SoundData obj = new SoundData();
        //_test = SoundData.AlarmSound;
    }
}