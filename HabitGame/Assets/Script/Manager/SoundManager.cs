using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Note : 책임 감소
// 처음 개발할 때는 SoundManager가 모든 SoundData를 관리하도록 설계를 했지만
// 이제는 SoundManager는 Presenter로 부터 AudioClip을 받고 재생하는 기능만 책임진다.
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
    
    #endregion

    #region 4. Methods

    

    private void BindEvent()
    {
    }

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

        //_musicPlayerMono.AudioClip = SoundData.AlarmChickenAudioClip;
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