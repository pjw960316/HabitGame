using System;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
    #region 1. Fields
    // default
    #endregion
    
    #region 2. Properties
    [SerializeField]
    private SoundData _soundData;
    public SoundData SoundData => _soundData;

    #endregion
    
    #region 3. Constructor

    private void Awake()
    {
        Debug.Log("Awake SOManager" );
        if (_soundData == null)
        {
            Debug.Log("No sound data loaded");
        }
    }

    #endregion

    #region 4. Methods
    // default
    #endregion
}
