using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;

public class UIRoutineRecordWidget : UIWidgetBase
{
    #region 1. Fields

    [SerializeField] private UIImageBase _dateWidget;
    [SerializeField] private List<UIImageBase> _routineRecordWidget;
    [SerializeField] private UIImageBase _routineRecordMoneyWidget;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;
    [SerializeField] private RectTransform _rectTransform;

    private MyCharacterManager _myCharacterManager;
    private Transform _transform;
    private bool _isUpdated = false;

    #endregion

    #region 2. Properties

    public RectTransform RectTransform => _rectTransform;

    public float WorldPosY => _transform.position.y;
    public string Date { get; private set; }

    public bool IsUpdated => _isUpdated;

    #endregion

    #region 3. Constructor

    public sealed override void OnAwake()
    {
        base.OnAwake();

        Initialize();
    }

    private void Initialize()
    {
        _myCharacterManager = MyCharacterManager.Instance;
        _transform = transform;
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // note
    // Auto-Texting 되는 것 존재.
    public void UpdateData(KeyValuePair<string, ImmutableList<bool>> routineRecordElement)
    {
        _isUpdated = true;
        
        Date = routineRecordElement.Key;
        var formattedDateString = $"{Date.Substring(0, 4)} / {Date.Substring(4, 2)} / {Date.Substring(6, 2)}"; // 날짜 포맷용 매직 넘버
        
        _dateWidget.SetText(formattedDateString);

        var routineCheckList = routineRecordElement.Value;
        var routineCheckCount = routineCheckList.Count;
        var successCount = 0;

        for (var index = 0; index < routineCheckCount; index++)
        {
            if (routineCheckList[index])
            {
                _routineRecordWidget[index].SetColor(_successColor);
                successCount++;
            }
            else
            {
                _routineRecordWidget[index].SetColor(_failColor);
            }
        }

        var moneyText = (_myCharacterManager.GetMoneyPerRoutineSuccess() * successCount).ToString("N0");
        _routineRecordMoneyWidget.SetText(moneyText);
    }

    public float GetAnchoredPositionY()
    {
        return _rectTransform.anchoredPosition.y;
    }

    #endregion
}