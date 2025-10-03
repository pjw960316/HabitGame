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

    private SoundData _soundData;
    private MusicPlayerController _mainMusicPlayerController;

    #endregion

    #region 2. Properties

    public SoundData SoundData => _soundData;

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
        //
    }

    public void Initialize()
    {
    }

    public void LateInitialize()
    {
        BindEvent();
    }

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
                ExceptionHelper.CheckNullException(_soundData, "SoundData");

                return;
            }
        }
    }

    public void SetMusicPlayerMono(MusicPlayerController musicPlayerController)
    {
        _mainMusicPlayerController = musicPlayerController;
        ExceptionHelper.CheckNullException(_mainMusicPlayerController, "_mainMusicPlayerController");
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    public void PlayBackgroundMusic()
    {
        _mainMusicPlayerController.UpdateAudioClipAndPlay(_soundData.BackgroundAudioClip);
    }

    public void PlayMusic(AudioClip audioClip)
    {
        _mainMusicPlayerController.UpdateAudioClipAndPlay(audioClip);
    }

    public void StopPlayMusic()
    {
        _mainMusicPlayerController.StopPlayMusic();
    }

    public void SetAudioSourceLoopOn()
    {
        _mainMusicPlayerController.SetLoopOn();
    }
    

    public void Dispose()
    {
    }

    #endregion
}