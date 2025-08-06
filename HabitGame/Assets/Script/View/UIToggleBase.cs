using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Note
// Toggle, Button 같은 친구들은 일단 Presenter를 만들지 않는다.
// 자신을 들고 있는 popup의 presenter를 이용한다.
public class UIToggleBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private UIPopupBase _parentPopup;
    
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _checkBoxBackgroundImg;
    [SerializeField] private Image _checkBoxCheckMarkImg;

    #endregion

    #region 2. Properties
    
    // default

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        OnAwake();
    }

    public void OnAwake()
    {
        Initialize();
    }

    // todo : 지우기
    public void CreatePresenterByManager()
    {
        //throw new System.NotImplementedException();
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
        BindEvent();
    }

    private void BindEvent()
    {
        _toggle.onValueChanged.AddListener(_ =>
        {
            
        });
    }

    public Toggle GetToggle()
    {
        return _toggle;
    }
    
    #endregion

    #region 5. EventHandlers
    
    // default

    #endregion
}