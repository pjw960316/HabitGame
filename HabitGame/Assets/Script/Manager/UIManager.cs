using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/* NOTE
 Runtime의 UI를 전역으로 관리하는 책임이 부여된 Singleton
 열려 있는 UI를 동적으로 관리해야 한다.
*/
public class UIManager : ManagerBase<UIManager>, IManager
{
    #region 1. Fields

    //test
    private Canvas _testCanvas;

    // REFACTOR
    private UIPopupData _popupData;

    #endregion

    #region 2. Properties

    public ViewData ViewData { get; private set; }
    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
    }

    public void SetModel(IEnumerable<ScriptableObject> _list)
    {
        foreach (var scriptableObject in _list)
        {
            if (scriptableObject is UIPopupData uiPopupData)
            {
                _popupData = uiPopupData;
            }

            if (scriptableObject is ViewData viewData)
            {
                ViewData = viewData;
            }
        }

        if (_popupData == null)
        {
            throw new InvalidOperationException("_popupData는 null이 될 수 없습니다. 올바른 데이터를 확인해주세요.");
        }

        if (ViewData == null)
        {
            throw new InvalidOperationException("_viewData null이 될 수 없습니다. 올바른 데이터를 확인해주세요.");
        }
    }

    // REFACTOR
    // String으로 받는 건 좋지 않지만...
    public void OpenPopupByStringKey(string key, Transform transform)
    {
        var popupPrefab = _popupData.GetPopupByStringKey(key);

        if (popupPrefab == null)
        {
            throw new InvalidOperationException("<UNK> <UNK> <UNK> <UNK> <UNK> <UNK>.");
        }

        Object.Instantiate(popupPrefab, transform);
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}