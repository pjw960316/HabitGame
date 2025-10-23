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
    private MainMusicController _mainMusicPlayerController;
    private UISFXController _uiSFXController;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
    }

    public void Initialize()
    {
    }

    public void LateInitialize()
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

    public void SetMainMusicPlayerController(MainMusicController musicPlayerController)
    {
        _mainMusicPlayerController = musicPlayerController;
        ExceptionHelper.CheckNullException(_mainMusicPlayerController, "_mainMusicPlayerController");
    }

    public void SetUISFXController(UISFXController SFXController)
    {
        _uiSFXController = SFXController;
        ExceptionHelper.CheckNullException(_uiSFXController, "_uiSFXController");
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6-1. MainMusicPlayer_Methods

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

    #endregion

    #region 6-2. UISFX_Methods

    public void PlaySFX(AudioClip audioClip)
    {
        _uiSFXController.UpdateAudioClipAndPlay(audioClip);
    }

    public void PlayCurrentSFX()
    {
        _uiSFXController.PlayCurrentAudioClip();
    }

    public void StopSFX()
    {
        _uiSFXController.StopPlayMusic();
    }

    public void SetSFXLoopOn()
    {
        _uiSFXController.SetLoopOn();
    }

    #endregion

    #region 6-3. Methods

    public void Dispose()
    {
    }

    #endregion
}