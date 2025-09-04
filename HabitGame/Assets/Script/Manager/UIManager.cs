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
    
    private PopupData _popupData;
    
    private readonly Dictionary<EPopupKey, UIPopupBase> _popupDictionary = new();

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
        var presenter = new TPresenter();
        presenter.Initialize(view);
    }
    
    public void OpenPopupByStringKey(EPopupKey key, Transform transform)
    {
        var popupPrefab = _popupData.GetPopupByEPopupKey(key);

        if (popupPrefab == null)
        {
            throw new InvalidOperationException("<UNK> <UNK> <UNK> <UNK> <UNK> <UNK>.");
        }

        var popup = Object.Instantiate(popupPrefab, transform).GetComponent<UIPopupBase>();
        
        if(popup != null)
        {
            // note
            // TryAdd를 사용하면, 최초로 생성한 Popup을 계속 Key의 Value로 저장합니다.
            // 그러면 그 popup을 끌 때 Value가 null이 되므로 
            // 매 번 새로 value를 갱신해주여 합니다.
            _popupDictionary[key] = popup;
        }
    }
    
    public TPopup GetPopupByStringKey<TPopup>(EPopupKey key) where TPopup : UIPopupBase
    {
        _popupDictionary.TryGetValue(key, out var popup);
        var castedPopup = popup as TPopup;

        return castedPopup;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}