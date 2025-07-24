using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
public class SoundData : ScriptableObject, IModel
{
    #region 1. Fields

    [SerializeField] private AudioClip _alarmChickenAudioClip;
    [SerializeField] private AudioClip _firstSleepingAudioClip;

    #endregion

    #region 2. Properties

    public AudioClip AlarmChickenAudioClip => _alarmChickenAudioClip;
    public AudioClip FirstSleepingAudioClip => _firstSleepingAudioClip;

    #endregion

    #region 3. Constructor

    // default

    #endregion

    #region 4. Methods

    // default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}