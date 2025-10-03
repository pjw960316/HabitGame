using System;
using UnityEngine;

public class MusicPlayerController : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] private AudioSource _audioSource;

    private readonly SoundManager _soundManager = SoundManager.Instance;

    #endregion

    #region 2. Properties

    public AudioSource AudioSource => _audioSource;

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        if (_soundManager == null)
        {
            throw new NullReferenceException("_soundManager");
        }

        _soundManager.SetMusicPlayerMono(this);
        _soundManager.PlayBackgroundMusic();
    }

    #endregion

    #region 4. Methods

    public void UpdateAudioClipAndPlay(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void StopPlayMusic()
    {
        _audioSource.Stop();
    }

    public void SetLoopOn()
    {
        _audioSource.loop = true;
    }

    #endregion
}