using System;
using System.Collections.Generic;
using UnityEngine;

/* NOTE
 Runtime의 UI를 전역으로 관리하는 책임이 부여된 Singleton
*/
public class UIManager : ManagerBase<UIManager>, IManager
{
    #region 1. Fields

    // REFACTOR
    // IModel 생각
    private UIPopupData _popupData;

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

    public void SetModel(IEnumerable<ScriptableObject> _list)
    {
        foreach (var scriptableObject in _list)
        {
            if (scriptableObject is UIPopupData uiPopupData)
            {
                _popupData = uiPopupData;
                return;
            }
        }
        if (_popupData == null)
        {
            throw new InvalidOperationException("_popupData는 null이 될 수 없습니다. 올바른 데이터를 확인해주세요.");
        }
    }
    
    public void ConnectInstanceByActivator(IManager instance)
    {
        if (_instance == null)
        {
            _instance = instance as UIManager;
        }
    }

    // REFACTOR
    // String으로 받는 건 좋지 않지만...
    public void OpenPopupByStringKey(string key)
    {
        var popupPrefab = _popupData.GetPopupByStringKey(key);
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}