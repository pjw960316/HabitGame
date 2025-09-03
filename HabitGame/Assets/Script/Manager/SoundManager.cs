using System;
using System.Collections.Generic;
using UnityEngine;

// Note : 책임 감소
// 처음 개발할 때는 SoundManager가 모든 SoundData를 관리하도록 설계를 했지만
// 이제는 SoundManager는 Presenter로 부터 AudioClip을 받고 재생하는 기능만 책임진다.
[Serializable]
public class SoundManager : ManagerBase<SoundManager>, IManager, IDisposable
{
    #region 1. Fields

    private MusicPlayerMono _musicPlayerMono;
    private AudioSource _audioSource;

    #endregion

    #region 2. Properties

    private SoundData _soundData;

    public SoundData SoundData
    {
        get
        {
            if (_soundData != null)
            {
                return _soundData;
            }

            throw new NullReferenceException("_soundData is Null");
        }
    }

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
        //
    }

    public void Initialize()
    {
        BindEvent();
    }

    private void BindEvent()
    {
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    public void RequestPlaySleepingMusic(AudioClip latestSleepingAudioClip)
    {
        _audioSource.clip = latestSleepingAudioClip;
        _musicPlayerMono.PlayMusic();
    }

    public void RequestPlayLoudAlarmMusic(AudioClip LoudAlarmAudioClip)
    {
        RequestStopPlayMusic();
        
        _audioSource.clip = LoudAlarmAudioClip;
        _musicPlayerMono.PlayMusic();
    }

    public void RequestStopPlayMusic()
    {
        _audioSource.Stop();
    }
    
    public void RequestAudioSourceLoopOn()
    {
        _audioSource.loop = true;
    }

    #endregion

    #region 6. Methods

    public void SetModel(IEnumerable<IModel> models)
    {
        foreach (var model in models)
        {
            if (model is SoundData soundData)
            {
                _soundData = soundData;

                return;
            }
        }
    }

    // note
    // mono 객체가 Awake 될 때 Set 되므로
    // 시점은 올바르다.
    public void SetMusicPlayerMono(MusicPlayerMono musicPlayerMono)
    {
        ExceptionHelper.CheckNullException(musicPlayerMono, "params : MusicPlayerMono");
        
        _musicPlayerMono = musicPlayerMono;
        _audioSource = _musicPlayerMono.AudioSource;
    }

    public void Dispose()
    {
    }

    #endregion
}