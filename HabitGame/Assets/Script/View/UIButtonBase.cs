using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private EStringKey _buttonTextKey;

    private readonly Subject<EPopupKey> _onClickButton = new();
    public IObservable<EPopupKey> OnClickButton => _onClickButton;

    #endregion

    #region 2. Properties

    public Canvas Canvas { get; private set; }

    #endregion

    #region 3. Constructor

    public void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
        BindEvent();
        SetButtonText();
    }

    #endregion

    #region 4. Methods

    // Note
    // Virtual로 변경하지 마세요.
    // 모든 상속 구조에서 Binding은 독립적으로 각각 실행되어야 합니다.
    private void BindEvent()
    {
        _button.onClick.AddListener(() => _onClickButton.OnNext(default));
    }

    private void SetButtonText()
    {
        ExceptionHelper.CheckNullException(_buttonText, "buttonText");

        _buttonText.text = StringManager.Instance.GetUIString(_buttonTextKey);
    }

    #endregion

    #region 5. EventHandlers

    //

    #endregion
}