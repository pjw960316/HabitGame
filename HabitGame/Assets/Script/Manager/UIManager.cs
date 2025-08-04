using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// TODO
// 무엇이 열려있고, 이런 거 관리해야 한다.
// 열린 View와 Presenter 관리
public class UIManager : ManagerBase<UIManager>, IManager
{
    #region 1. Fields

    // REFACTOR
    private PopupData _popupData;

    #endregion

    #region 2. Properties
    // REFACTOR
    // can be null? - view로 받기 때문에.
    public Canvas MainCanvas { get; private set; } 
    public ViewData ViewData { get; private set; }

   

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        BindEvent();
    }

    public void GetModel()
    {
        
    }

    #endregion

    #region 4. Methods

    
    private void BindEvent()
    {
    }

    public void SetModel(IEnumerable<IModel> _list)
    {
        foreach (var scriptableObject in _list)
        {
            if (scriptableObject is PopupData uiPopupData)
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
    
    /*public void InjectMainCanvas(Canvas canvas)
    {
        MainCanvas = canvas;
    }*/
    
    public void CreatePresenter<TPresenter>(IView view) where TPresenter : IPresenter, new()
    {
        TPresenter presenter = new TPresenter();
        presenter.Initialize(view);
    }
    
    public void OpenPopupByStringKey(EPopupKey key, Transform transform)
    {
        var popupPrefab = _popupData.GetPopupByEPopupKey(key);

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