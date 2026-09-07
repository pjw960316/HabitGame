using TMPro;
using UnityEngine;
using UnityEngine.UI;

// NOTE
/*
 1. 역할
 이미지 + 텍스트 or 이미지 단독의 위젯에 붙여서 사용한다.

 2. 기본 이미지
 가장 기본이미지라면 상속받지 않고, 이걸 바로 쓰도록 하자 -> abstract 사용 X
*/
public class UIImageBase : UIWidgetBase
{
    #region 1. Fields

    [SerializeField] private TextMeshProUGUI _imageText;
    [SerializeField] private EStringKey _imageTextKey;
    [SerializeField] protected Image _image;

    // NOTE
    // true면 EStringKey를 이용해 고정 문구를 자동 설정하고,
    // false면 날짜, 금액, 포인트처럼 실행 중 결정되는 값을 SetText()로 설정한다.
    [SerializeField] private bool _isAutoSetText;

    #endregion

    #region 2. Properties

    public Image Image => _image;

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();

        Initialize();

        BindEvent();
    }

    private void Initialize()
    {
        if (_isAutoSetText)
        {
            SetAutoText();
        }
    }

    private void BindEvent()
    {
    }

    #endregion

    #region 4. Methods

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