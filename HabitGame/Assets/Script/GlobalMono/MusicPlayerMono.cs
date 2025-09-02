using System;
using UnityEngine;

// TODO
// 인터페이스로 묶어서 이런 음악 재생기에게 책임을 부여해라.
public class MusicPlayerMono : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] private AudioSource _audioSource;
    
    private readonly SoundManager _soundManager = SoundManager.Instance;

    #endregion

    #region 2. Properties

    public AudioSource AudioSource
    {
        get => _audioSource;
        private set => _audioSource = value;
    }

    public AudioClip AudioClip
    {
        get => _audioSource.clip;
        set => _audioSource.clip = value;
    }

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        if (_soundManager == null)
        {
            throw new NullReferenceException("_soundManager");
        }
        
        _soundManager.SetMusicPlayerMono(this);
    }

    #endregion

    #region 4. Methods
    public void PlayMusic()
    {
        _audioSource?.Play();
    }
    #endregion
}