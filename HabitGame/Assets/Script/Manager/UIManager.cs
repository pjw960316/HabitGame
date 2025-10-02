using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

// note
// 열려있는 View를 관리한다.
public class UIManager : ManagerBase<UIManager>, IManager
{
    #region 1. Fields

    private PopupData _popupData;

    private readonly Dictionary<EPopupKey, UIPopupBase> _popupDictionary = new();

    private readonly Subject<EPopupKey> _onOpenPopup = new();
    private readonly Subject<EPopupKey> _onClosePopup = new();
    
    public readonly HashSet<EPopupKey> _openedPopupKeyList = new();
    public readonly HashSet<EPopupKey> _pendingPopupKeyList = new();

    #endregion

    #region 2. Properties

    public Canvas MainCanvas { get; private set; }

    public Transform MainCanvasTransform { get; private set; }

    public Subject<EPopupKey> OnOpenPopup => _onOpenPopup;
    public IObservable<EPopupKey> OnClosePopup => _onClosePopup;

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
    }

    public void Initialize()
    {
        BindEvent();
    }

    private void BindEvent()
    {
        // todo
        // Manager의 Disposable 정책
        OnClosePopup.Subscribe(RemoveOpenedPopup);
    }

    #endregion


    #region 4. EventHandlers

    private void RemoveOpenedPopup(EPopupKey ePopupKey)
    {
        if (_openedPopupKeyList.Contains(ePopupKey))
        {
            _openedPopupKeyList.Remove(ePopupKey);
        }

        if (IsAnyPopupOpened() == false)
        {
            RequestUpdateCameraPos();
        }
    }

    #endregion

    #region 5. Request Methods

    public void AddPendingPopup(EPopupKey ePopupKey)
    {
        _pendingPopupKeyList.Add(ePopupKey);
    }

    // test
    private void RequestUpdateCameraPos()
    {
        CameraManager.Instance.RequestMainCameraDispose();
    }

    #endregion

    #region 6. Methods

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

    public void SetMainCanvas(UIMainCanvas canvas)
    {
        ExceptionHelper.CheckNullException(canvas, "MainCanvas Inject Fail");

        MainCanvas = canvas.MainCanvas;
        MainCanvasTransform = MainCanvas.transform;
    }

    public void OpenPopupByStringKey(EPopupKey key, Transform transform)
    {
        var popupPrefab = _popupData.GetPopupByEPopupKey(key);

        if (popupPrefab == null)
        {
            throw new InvalidOperationException("<UNK> <UNK> <UNK> <UNK> <UNK> <UNK>.");
        }

        var popup = Object.Instantiate(popupPrefab, transform).GetComponent<UIPopupBase>();

        if (popup != null)
        {
            // note
            // TryAdd를 사용하면, 최초로 생성한 Popup을 계속 Key의 Value로 저장합니다.
            // 그러면 그 popup을 끌 때 Value가 null이 되므로 
            // 매 번 새로 value를 갱신해주여 합니다.
            _popupDictionary[key] = popup;
            _openedPopupKeyList.Add(key);
            _pendingPopupKeyList.Remove(key);
        }
    }

    public TPopup GetPopupByStringKey<TPopup>(EPopupKey key) where TPopup : UIPopupBase
    {
        _popupDictionary.TryGetValue(key, out var popup);
        var castedPopup = popup as TPopup;

        return castedPopup;
    }

    public bool IsPopupOpeningOrOpened(EPopupKey key)
    {
        if (_openedPopupKeyList.Contains(key))
        {
            return true;
        }

        if (_pendingPopupKeyList.Contains(key))
        {
            return true;
        }

        return false;
    }

    public bool IsAnyPopupOpened()
    {
        if (_openedPopupKeyList.Any())
        {
            return true;
        }

        return false;
    }

    public bool IsAnyPopupOpenedOrPending()
    {
        if (IsAnyPopupOpened())
        {
            return true;
        }

        if (_pendingPopupKeyList.Any())
        {
            return true;
        }

        return false;
    }

    #endregion
}