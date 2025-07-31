using AYellowpaper.SerializedCollections;
using UnityEngine;


// Note
// Popup을 열기 위함
[CreateAssetMenu(fileName = "PopupData", menuName = "ScriptableObjects/PopupData")]
public class PopupData : ScriptableObject, IModel
{
    #region 1. Fields

    [SerializeField] private SerializedDictionary<EPopupKey, GameObject> _popupPrefabDict = new();

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

    public GameObject GetPopupByEPopupKey(EPopupKey key)
    {
        return  _popupPrefabDict[key];
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion

    //test
}

public enum EPopupKey
{
    AlarmPopup,
    RoutineCheckPopup,
}