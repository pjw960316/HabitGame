using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class SoundManager : ManagerBase<SoundManager>, IManager, IDisposable
{
    #region 1. Fields

    private CompositeDisposable _disposable = new();
    public Subject<int> TestEvent;

    #endregion

    #region 2. Properties

    [SerializeField] private AudioSource musicPlayer;

    private SoundData _soundData;

    public SoundData SoundData
    {
        get => _soundData;
        private set => _soundData = value;
    }

    #endregion

    #region 3. Constructor

    public SoundManager()
    {
        //LOG
        Debug.Log("SoundManager Constructor");

        TestEvent = new Subject<int>();
    }

    #endregion

    #region 4. Methods

    public void Init()
    {
        BindEvent();
    }

    private void BindEvent()
    {
        TestEvent.Subscribe(x => { Debug.Log(x.ToString()); }).AddTo(_disposable);
    }


    public void SetModel(IEnumerable<ScriptableObject> models)
    {
        foreach (var model in models)
        {
            if (model is SoundData soundData)
            {
                SoundData = soundData;
                return;
            }
        }
    }

    // FIX
    // 다형성 쓰면 뭔가 연결 될 것 같은데?????
    public void ConnectViewWithPresenter(IView view, IPresenter presenter)
    {
        //일단 테스트
        var alarmPresenter = new AlarmPresenter(view, _soundData);
        if (view is UIAlarmButton uiAlarmButton)
        {
            uiAlarmButton.InjectPresenter(alarmPresenter);
        }
    }

    public void InitializeScriptableObject(IModel data)
    {
        if (data is SoundData soundData)
        {
            _soundData = soundData;
        }
    }

    public void ConnectInstanceByActivator(IManager instance)
    {
        if (_instance == null)
        {
            _instance = instance as SoundManager;
        }
    }

    // TODO : 제거하거나 위치를 옮기자.
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

    public void Dispose()
    {
        Debug.Log("fuck");
    }

    #endregion
}