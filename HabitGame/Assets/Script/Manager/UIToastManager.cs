using System;
using UniRx;
using UnityEngine;

public class UIToastManager : ManagerBase<UIToastManager>
{
    #region 1. Fields

    public const float TOAST_MESSAGE_LIFE_TIME = 3f;
    private GameObject _toastMessage;
    private Transform _toastMessageTransform;

    #endregion

    #region 2. Properties

    public UIMainCanvas MainCanvas { get; private set; }

    #endregion

    #region 3. Constructor

    public void SetMainCanvas(UIMainCanvas canvas)
    {
        MainCanvas = canvas;
        _toastMessage = MainCanvas.ToastMessage.gameObject;
        _toastMessageTransform = _toastMessage.transform;
    }

    #endregion

    #region 4. Methods

    public void ShowToast(EToastStringKey eToastStringKey)
    {
        UpdateToastText(eToastStringKey);

        PresentToast();
    }

    public void ShowToast(EToastStringKey eToastStringKey, params object[] args)
    {
        UpdateToastText(eToastStringKey, args);
        PresentToast();
    }

    private void UpdateToastText(EToastStringKey eToastStringKey)
    {
        var toastString = StringManager.Instance.GetToastString(eToastStringKey);

        MainCanvas.ToastText.text = toastString;
    }

    private void UpdateToastText(EToastStringKey eToastStringKey, params object[] args)
    {
        var toastString = StringManager.Instance.GetToastString(eToastStringKey);

        MainCanvas.ToastText.text = string.Format(toastString, args);
    }

    private void PresentToast()
    {
        _toastMessageTransform.SetAsLastSibling();
        _toastMessage.SetActive(true);

        RemoveToastMessage(_toastMessage);
    }

    private void RemoveToastMessage(GameObject toastMessage)
    {
        Observable.Timer(TimeSpan.FromSeconds(TOAST_MESSAGE_LIFE_TIME))
            .Subscribe(_ =>
            {
                if (toastMessage == null)
                {
                    return;
                }

                toastMessage.SetActive(false);
            });
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}