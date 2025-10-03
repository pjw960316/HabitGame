using AYellowpaper.SerializedCollections;
using UnityEngine;

// note : 게임 전체에서 사용되는 Sound
[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
public class SoundData : ScriptableObject, IModel
{
    #region 1. Fields

    [SerializeField] private AudioClip _backgroundAudioClip;

    #endregion

    #region 2. Properties

    public AudioClip BackgroundAudioClip => _backgroundAudioClip;

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