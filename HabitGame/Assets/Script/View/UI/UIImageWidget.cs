using TMPro;
using UnityEngine;
using UnityEngine.UI;

// NOTE
/*
 1. 역할
 이미지 + 텍스트 or 이미지 단독의 위젯에 붙여서 사용한다.

 2. 책임
 자신의 상위 Popup으로부터 필요한 데이터를 전달받으면 그걸 UI에서 변경하는 책임만 존재한다.
*/
public class UIImageWidget : UIWidgetBase
{
    #region 1. Fields

    [SerializeField] protected Image _image;
    [SerializeField] private TextMeshProUGUI _imageText;
    [SerializeField] private EStringKey _imageTextKey;
    

    // NOTE
    // T -> 고정 문구 (EStringKey 기반)
    // F -> 런타임에 변경되는 문구
    [SerializeField] private bool _isAutoSetText;

    #endregion

    #region 2. Properties

    //

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