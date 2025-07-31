using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundManager : ManagerBase<SoundManager>, IManager, IDisposable
{
    #region 1. Fields

    private MusicPlayerMono _musicPlayerMono;

    #endregion

    #region 2. Properties

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
        //Debug.Log("SoundManager Constructor");
    }

    #endregion

    #region 4. Methods

    public void Initialize()
    {
        BindEvent();
    }

    private void BindEvent()
    {
    }


    public void SetModel(IEnumerable<ScriptableObject> models)
    {
        foreach (var model in models)
        {
            if (model is SoundData soundData)
            {
                // LOG
                Debug.Log("SoundManager Sets SoundData");
                
                SoundData = soundData;
                return;
            }
        }
    }

    // refactor
    // Manager에서 만들긴 해야 하는데 왜 SoundManager?
    public TPresenter GetPresenterAfterCreate<TPresenter>(IView view) where TPresenter : IPresenter, new()
    {
        var presenter = new TPresenter();
        presenter.Initialize(view);

        return presenter;
    }
    

    public void RegisterMusicPlayerMono(MusicPlayerMono musicPlayerMono)
    {
        _musicPlayerMono = musicPlayerMono;

        if (_musicPlayerMono == null)
        {
            throw new NullReferenceException("MusicPlayerMono is null");
        }
    }

    // NOTE
    // 추후에 원하는 musicPlayer로 음악을 커스텀 하게 재생 시킬 수 있다. (Generic)
    // SoundManager는 왕과 같은 존재고, 여러 개의 MusicPlayerMono들이 재생의 책임이 있는 Instance
    public void CommandPlayingMusic(AudioClip audioClip)
    {
        _musicPlayerMono.AudioClip = audioClip;
        _musicPlayerMono.PlayMusic();
    }

    public void CommandPlayingWakeUpSound()
    {
        _musicPlayerMono.AudioSource.Stop();

        _musicPlayerMono.AudioClip = SoundData.AlarmChickenAudioClip;
        _musicPlayerMono.PlayMusic();
    }

    public void SetAudioSourceLoop()
    {
        _musicPlayerMono.AudioSource.loop = true;
    }

    #endregion

    #region 5. EventHandlers

    public void Dispose()
    {
    }

    #endregion
}