using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "StringData", menuName = "ScriptableObjects/StringData")]
public class StringData : ScriptableObject, IModel
{
    #region 1. Fields

    [SerializeField] private SerializedDictionary<EStringKey, string> _stringDictionary = new();

    #endregion

    #region 2. Properties

    public SerializedDictionary<EStringKey, string> StringDictionary => _stringDictionary;

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    public string GetStringByEStringKey(EStringKey eStringKey)
    {
        return _stringDictionary[eStringKey];
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}

public enum EStringKey
{
    //test
    One
}