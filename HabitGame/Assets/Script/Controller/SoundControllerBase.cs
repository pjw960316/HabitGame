using UnityEngine;

public abstract class SoundControllerBase : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] protected AudioSource _audioSource;

    protected SoundManager _soundManager;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        _soundManager = SoundManager.Instance;

        ExceptionHelper.CheckNullException(_audioSource, "AudioSource is missing");
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void UpdateAudioClipAndPlay(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void PlayCurrentAudioClip()
    {
        ExceptionHelper.CheckNullException(_audioSource.clip, "Audioclip is missing");
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