using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    private readonly SoundManager _soundManager = SoundManager.Instance;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    private void Start()
    {
        _soundManager.SetAudioSource(_audioSource);

        //test
        _soundManager.SetAudioClip(_audioClip);
    }

    #endregion

    #region 4. Methods

    // default

    #endregion
}