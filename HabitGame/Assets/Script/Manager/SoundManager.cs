using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SoundManager : SingletonBase<SoundManager>, IManager
{
    #region 1. Fields

    private SoundData _soundData;
    // default

    #endregion

    #region 2. Properties
    [SerializeField] private AudioSource musicPlayer;

    #endregion

    #region 3. Constructor

    // default

    #endregion

    #region 4. Methods

    public void Init()
    {
    }

    // TODO : 다형성 쓰면 뭔가 연결 될 것 같은데?????
    public void ConnectViewWithPresenter(IView view, IPresenter presenter)
    {
        //일단 테스트
        AlarmPresenter alarmPresenter = new  AlarmPresenter(view , _soundData);
    }
    
    public void InitializeScriptableObject(IModel data)
    {
        if (data is SoundData soundData)
        {
            _soundData = soundData;
        }
        
        //TODO : test Code
        if (_soundData != null)
        {
            //Debug.Log("today end");
        }
    }

    // TODO : Erase - 여기 들어가기에는 너무 specific
    public IEnumerator Play()
    {
        Debug.Log("Play");
        musicPlayer.volume = 0.5f;
        musicPlayer.loop = false;
        //musicPlayer.PlayOneShot(thirtyMinutesSound);

        //yield return new WaitForSeconds(PLAY_TIME);

        Debug.Log("Stop");
        musicPlayer.Stop();

        musicPlayer.volume = 1f;
        //musicPlayer.clip = alarmSound;
        musicPlayer.loop = true;
        yield return new WaitForSeconds(0.5f);
        musicPlayer.Play();
    }

    public bool IsMusicPlaying()
    {
        return musicPlayer.isPlaying;
    }

    private void Test()
    {
    }

    #endregion
}