using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UIToastManager : ManagerBase<UIToastManager>, IManager
{
    #region 1. Fields

    public const float TOAST_MESSAGE_LIFE_TIME = 3f;

    #endregion

    #region 2. Properties

    // refactor
    // 특수 View에 대해서 고려해라. 얘는 어차피 게임 실행 중 상시 존재잖아.
    public UIMainCanvas MainCanvas { get; private set; }

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
    }

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    public void InjectMainCanvas(UIMainCanvas canvas)
    {
        MainCanvas = canvas;
    }

    public void SetModel(IEnumerable<IModel> models)
    {
    }

    public void ShowToast(EToastStringKey eToastStringKey)
    {
        var toastMessage = MainCanvas.ToastMessage.gameObject;
        ExceptionHelper.CheckNullException(toastMessage, "toastMessage");

        UpdateToastText(eToastStringKey);
        
        toastMessage.SetActive(true);

        RemoveToastMessage(toastMessage);
    }

    private void UpdateToastText(EToastStringKey eToastStringKey)
    {
        var toastString = StringManager.Instance.GetToastString(eToastStringKey);
        
        MainCanvas.ToastText.text = toastString;
    }

    //note
    //don't need disposable
    private void RemoveToastMessage(GameObject toastMessage)
    {
        Observable.Timer(TimeSpan.FromSeconds(TOAST_MESSAGE_LIFE_TIME))
            .Subscribe(_ => { toastMessage?.SetActive(false); });
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}