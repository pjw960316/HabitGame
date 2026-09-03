using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

// NOTE
// 버튼 아닌 위젯은 이미지 베이스라고 구현했다.
public class UIImageBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private TextMeshProUGUI _imageText;
    [SerializeField] private EStringKey _imageTextKey;
    [SerializeField] protected Image _image;

    // NOTE
    // true면 EStringKey를 이용해 고정 문구를 자동 설정하고,
    // false면 날짜, 금액, 포인트처럼 실행 중 결정되는 값을 SetText()로 설정한다.
    [SerializeField] private bool _isAutoSetText;

    protected UIManager _uiManager;

    protected readonly Subject<EPopupKey> _onClickButton = new();
    public IObservable<EPopupKey> OnClickButton => _onClickButton;

    #endregion

    #region 2. Properties

    public Image Image => _image;

    #endregion

    #region 3. Constructor

    public void Awake()
    {
        // NOTE
        // Overriding
        // Script가 UIButtonBase가 붙으면 Base의 OnAwake()가 호출되고
        // Script가 UIOpenPopupButtonBase가 붙으면 Derived의 OnAwake()가 호출되기 바람.
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        Initialize();

        // NOTE
        // Shadowing
        // Script가 UIButtonBase가 붙으면 Base의 BindEvent()가 호출되고
        // Script가 UIOpenPopupButtonBase가 붙어도 Derived의 BindEvent()가 호출되기 바람.
        BindEvent();
    }

    #endregion

    #region 4. Methods

    // NOTE
    // Virtual로 변경하지 마세요.
    // 모든 상속 구조에서 Binding은 독립적으로 각각 실행되어야 합니다.
    private void BindEvent()
    {
    }

    private void Initialize()
    {
        _uiManager = UIManager.Instance;
        
        if (_isAutoSetText)
        {
            SetAutoText();
        }
    }


    private void SetAutoText()
    {
        ExceptionHelper.CheckNullException(_imageText, "ImageText");

        _imageText.text = StringManager.Instance.GetUIString(_imageTextKey);
    }

    public void SetText(string text)
    {
        _imageText.text = text;
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }

    #endregion

    #region 5. EventHandlers

    //

    #endregion
}
