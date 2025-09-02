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

    private Canvas _mainCanvas;

    private Transform _mainCanvasTransform;
    
    // REFACTOR
    private PopupData _popupData;

    private Dictionary<Type, IPresenter> _presenterDictionary = new();

    #endregion

    #region 2. Properties

    public Canvas MainCanvas => _mainCanvas;

    public Transform MainCanvasTransform => _mainCanvasTransform;

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
    }

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
        }
        
        if (_popupData == null)
        {
            throw new InvalidOperationException("_popupData는 null이 될 수 없습니다. 올바른 데이터를 확인해주세요.");
        }
    }
    
    public void InjectMainCanvas(UIMainCanvas canvas)
    {
        ExceptionHelper.CheckNullException(canvas, "MainCanvas Inject Fail");
        
        _mainCanvas = canvas.MainCanvas;
        _mainCanvasTransform = _mainCanvas.transform;
    }
    
    public void CreatePresenter<TPresenter>(IView view) where TPresenter : IPresenter, new()
    {
        TPresenter presenter = new TPresenter();
        presenter.Initialize(view);
        
        _presenterDictionary.Add(presenter.GetType(), presenter);
    }

    //test
    public void ConnectViewAndPresenter(UIAlarmTimerPopup view, AlarmPresenter presenter)
    {
        
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
    
    //test
    public GameObject GetOpenPopupByStringKey(EPopupKey key, Transform transform)
    {
        var popupPrefab = _popupData.GetPopupByEPopupKey(key);

        if (popupPrefab == null)
        {
            throw new InvalidOperationException("<UNK> <UNK> <UNK> <UNK> <UNK> <UNK>.");
        }

        var popup= Object.Instantiate(popupPrefab, transform);
        return popup;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}