using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIRoutineRecordPopup : UIPopupBase
{
    public struct ScrollData
    {
        public readonly float Offset;
        public readonly bool IsScrollDown;
        public readonly UIRoutineRecordWidget MovingWidget;

        public ScrollData(float offset, bool isScrollDown, UIRoutineRecordWidget movingWidget)
        {
            Offset = offset;
            IsScrollDown = isScrollDown;
            MovingWidget = movingWidget;
        }
    }

    #region 1. Fields

    private const int WIDGET_SHOW_COUNT = 4;
    private const float WIDGET_SCROLL_UP_OFFSET = 2.5f;
    private const int WIDGET_SCROLL_DOWN_OFFSET = 3;

    [SerializeField] private GameObject _widgetPrefab;
    [SerializeField] private GameObject _scrollPanel;
    [SerializeField] private GameObject _viewPort;
    [SerializeField] private GameObject _contents;
    [SerializeField] private UIButtonBase _closeBtn;

    private List<UIRoutineRecordWidget> _widgetList;
    private ScrollRect _scrollRect;
    private RectTransform _contentsRectTransform;

    private float _widgetRectWidth;
    private float _widgetRectHeight;

    private float _viewPortWorldPosY;
    private float _currentVerticalNormalizedPosition;

    private readonly Subject<ScrollData> _onUpdateScrollWidget = new();
    private readonly Subject<Unit> _onCloseButtonClicked = new();

    #endregion

    #region 2. Properties

    public IObservable<ScrollData> OnUpdateScrollWidget => _onUpdateScrollWidget;

    public Subject<Unit> OnCloseButtonClicked => _onCloseButtonClicked;

    #endregion

    #region 3. Constructor

    protected sealed override void Initialize()
    {
        base.Initialize();

        _contentsRectTransform = _contents.GetComponent<RectTransform>();
        _currentVerticalNormalizedPosition = 1f;
        _viewPortWorldPosY = _viewPort.transform.position.y;
        
        InitializeEPopupKey();
        InitializeWidgetSettingData();
        CreateWidgets();

        _scrollRect = _scrollPanel.GetComponent<ScrollRect>();
        ExceptionHelper.CheckNullException(_scrollRect, "_scrollRect");
    }

    protected override void InitializeEPopupKey()
    {
        _ePopupKey = EPopupKey.RoutineRecordPopup;
    }

    private void InitializeWidgetSettingData()
    {
        _widgetList = new List<UIRoutineRecordWidget>();

        _widgetRectWidth = _contentsRectTransform.rect.width;
        _widgetRectHeight = _scrollPanel.GetComponent<RectTransform>().rect.height / WIDGET_SHOW_COUNT;
    }

    public void CreateWidgets()
    {
        for (var idx = 0; idx < WIDGET_SHOW_COUNT * 2f; idx++)
        {
            var widget = Instantiate(_widgetPrefab, _contents.transform).GetComponent<UIRoutineRecordWidget>();
            var widgetRect = widget.RectTransform;
            var heightOffset = -1 * idx * _widgetRectHeight;

            widgetRect.sizeDelta = new Vector2(_widgetRectWidth, _widgetRectHeight);
            widgetRect.anchoredPosition = new Vector3(0, heightOffset, 0);
            _widgetList.Add(widget);
        }
    }


    protected override void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter<RoutineRecordPresenter>(this);
    }

    protected override void BindEvent()
    {
        _scrollRect.onValueChanged.AddListener(_ => OnScroll());

        _closeBtn.OnClick.AddListener(() => { _onCloseButtonClicked.OnNext(default); });
    }

    #endregion

    #region 4. Event Handlers

    private void OnScroll()
    {
        UpdateWidgetIfNeeded();
    }

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    private void UpdateWidgetIfNeeded()
    {
        var isScrollDown = _scrollRect.verticalNormalizedPosition < _currentVerticalNormalizedPosition;
        var movingWidget = isScrollDown ? GetTopWidget() : GetBottomWidget();
        
        if (isScrollDown)
        {
            var shouldScrollUpdate = movingWidget.WorldPosY >
                                     _viewPortWorldPosY + _widgetRectHeight * WIDGET_SCROLL_DOWN_OFFSET;
            if (shouldScrollUpdate)
            {
                var data = new ScrollData(_widgetRectHeight, isScrollDown, movingWidget);
                _onUpdateScrollWidget?.OnNext(data);
            }
        }
        else
        {
            var shouldScrollUpdate = movingWidget.WorldPosY <
                                     _viewPortWorldPosY - _widgetRectHeight * WIDGET_SCROLL_UP_OFFSET;
            if (shouldScrollUpdate)
            {
                var data = new ScrollData(_widgetRectHeight, isScrollDown, movingWidget);
                _onUpdateScrollWidget?.OnNext(data);
            }
        }

        _currentVerticalNormalizedPosition = _scrollRect.verticalNormalizedPosition;
    }


    public UIRoutineRecordWidget GetTopWidget()
    {
        return GetNestedWidget(false);
    }

    public UIRoutineRecordWidget GetBottomWidget()
    {
        return GetNestedWidget(true);
    }

    // Note : GetTopWidget , GetBottomWidget Nested Method
    private UIRoutineRecordWidget GetNestedWidget(bool isBottom)
    {
        var currentY = _widgetList[0].GetAnchoredPositionY();
        var targetWidget = _widgetList[0];
        var isTop = !isBottom;

        foreach (var widget in _widgetList)
        {
            var widgetAnchoredPositionY = widget.GetAnchoredPositionY();
            if ((isBottom && currentY > widgetAnchoredPositionY) || (isTop && currentY < widgetAnchoredPositionY))
            {
                currentY = widgetAnchoredPositionY;
                targetWidget = widget;
            }
        }

        return targetWidget;
    }

    public void SetWidgetData(
        ImmutableSortedDictionary<string, ImmutableList<bool>> routineRecordDictionary)
    {
        var index = 0;
        var widgetListCount = _widgetList.Count;

        foreach (var element in routineRecordDictionary)
        {
            if (index == widgetListCount)
            {
                break;
            }

            _widgetList[index].UpdateData(element);
            index++;
        }
    }

    public void UpdateWidgetData(UIRoutineRecordWidget movingWidget, KeyValuePair<string, ImmutableList<bool>> routineRecordData)
    {
        movingWidget.UpdateData(routineRecordData);
    }
    
    public void ShowTopContent()
    {
        _scrollRect.verticalNormalizedPosition = 1f;
    }

    // note : Contents의 Height는 Presenter로 부터 데이터의 개수에 맞게 세팅
    public void SetContentsHeight(int finalShowWidgetCount)
    {
        _contentsRectTransform.sizeDelta =
            new Vector2(_contentsRectTransform.rect.width, _widgetRectHeight * finalShowWidgetCount);
        
        Debug.Log($"{_widgetRectWidth} ");
    }

    public int GetWidgetShowCount()
    {
        return WIDGET_SHOW_COUNT;
    }

    #endregion
}