using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

public class UIManager : ManagerBase<UIManager>, IManager
{
    #region 1. Fields

    private PopupData _popupData;
    private Canvas _mainCanvas;
    private Transform _mainCanvasTransform;

    private readonly HashSet<EPopupKey> _openedPopupKeyList = new();
    private readonly Dictionary<EPopupKey, UIPopupBase> _popupDictionary = new();

    private readonly Subject<EPopupKey> _onOpenPopup = new();
    private readonly Subject<EPopupKey> _onClosePopup = new();

    #endregion

    #region 2. Properties

    public Canvas MainCanvas => _mainCanvas;
    public Transform MainCanvasTransform => _mainCanvasTransform;

    public IObservable<EPopupKey> OnOpenPopup => _onOpenPopup;

    public Subject<EPopupKey> OnClosePopup => _onClosePopup;

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
    }

    public void Initialize()
    {
    }

    public void LateInitialize()
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

    public void SetMainCanvas(UIMainCanvas canvas)
    {
        ExceptionHelper.CheckNullException(canvas, "MainCanvas Inject Fail");

        _mainCanvas = canvas.MainCanvas;
        _mainCanvasTransform = MainCanvas.transform;
    }

    #endregion


    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    public void OpenPopupByStringKey(EPopupKey popupKey, Transform transform)
    {
        var popupPrefab = _popupData.GetPopupByEPopupKey(popupKey);

        if (popupPrefab == null)
        {
            throw new InvalidOperationException("<UNK> <UNK> <UNK> <UNK> <UNK> <UNK>.");
        }

        var popup = Object.Instantiate(popupPrefab, transform).GetComponent<UIPopupBase>();

        if (popup != null)
        {
            _popupDictionary[popupKey] = popup;
            _openedPopupKeyList.Add(popupKey);

            _onOpenPopup.OnNext(popupKey);
        }
    }

    public void RemoveOpenedPopup(EPopupKey popupKey)
    {
        if (_openedPopupKeyList.Contains(popupKey))
        {
            _openedPopupKeyList.Remove(popupKey);

            _onClosePopup.OnNext(popupKey);
        }
    }

    public bool IsAnyPopupOpened()
    {
        if (_openedPopupKeyList.Any())
        {
            return true;
        }

        return false;
    }

    #endregion
}