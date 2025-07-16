using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
public class SoundData : ScriptableObject, IModel
{
    #region 1. Fields
    
    //default

    #endregion

    #region 2. Properties

    [SerializeField] private AudioClip _alarmSound;
    public AudioClip AlarmSound => _alarmSound;

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