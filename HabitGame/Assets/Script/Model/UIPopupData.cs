using UnityEngine;

[CreateAssetMenu(fileName = "UIPopupData", menuName = "ScriptableObjects/UIPopupData")]
public class UIPopupData : ScriptableObject, IModel
{
    #region 1. Fields

    [SerializeField] private SerializableDictionary<string, GameObject> _popupPrefabDict = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    public GameObject GetPopupByStringKey(string key)
    {
        return  _popupPrefabDict[key];
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion

    //test
}