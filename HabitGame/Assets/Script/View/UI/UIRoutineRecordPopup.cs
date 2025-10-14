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

    private const float WIDGET_SCROLL_UP_OFFSET = 2f;
    private const float WIDGET_SCROLL_DOWN_OFFSET = 4f;
    private const int WIDGET_SHOW_COUNT = 4;

    [SerializeField] private GameObject _widgetPrefab;
    [SerializeField] private GameObject _scrollPanel;
    [SerializeField] private GameObject _viewPort;
    [SerializeField] private GameObject _contents;
    [SerializeField] private UIButtonBase _closeBtn;

    private List<UIRoutineRecordWidget> _widgetList;
    private ScrollRect _scrollRect;

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

        InitializeEPopupKey();
        InitializeWidgets();

        _scrollRect = _scrollPanel.GetComponent<ScrollRect>();
        ExceptionHelper.CheckNullException(_scrollRect, "_scrollRect");


        _currentVerticalNormalizedPosition = 1f;
        _viewPortWorldPosY = _viewPort.transform.position.y;
    }

    protected override void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter<RoutineRecordPresenter>(this);
    }

    protected override void InitializeEPopupKey()
    {
        _ePopupKey = EPopupKey.RoutineRecordPopup;
    }

    private void InitializeWidgets()
    {
        _widgetList = new List<UIRoutineRecordWidget>();

        _widgetRectWidth = _contents.GetComponent<RectTransform>().rect.width;
        _widgetRectHeight = _scrollPanel.GetComponent<RectTransform>().rect.height / WIDGET_SHOW_COUNT;
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

        UpdateCurrentVerticalNormalizedPosition();
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
    }

    private void UpdateCurrentVerticalNormalizedPosition()
    {
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

    public void InitializeContentsHeight(int widgetCount)
    {
        _contents.GetComponent<RectTransform>().sizeDelta =
            new Vector2(GetComponent<RectTransform>().rect.width, _widgetRectHeight * widgetCount);
    }

    public void CreateRoutineRecordWidgetPrefabs(int widgetCount)
    {
        for (var index = 0; index < widgetCount; index++)
        {
            var routineRecordWidget =
                Instantiate(_widgetPrefab, _contents.transform).GetComponent<UIRoutineRecordWidget>();


            routineRecordWidget.RectTransform.sizeDelta =
                new Vector2(_widgetRectWidth, _widgetRectHeight);

            _widgetList.Add(routineRecordWidget);

            routineRecordWidget.RectTransform.anchoredPosition =
                new Vector3(0, -(index * _widgetRectHeight), 0);
        }
    }

    public void InitializeRoutineRecordWidgets(
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

    public void ShowTopContent()
    {
        _scrollRect.verticalNormalizedPosition = 1f;
    }

    public void UpdateWidgetData(UIRoutineRecordWidget movingWidget,
        KeyValuePair<string, ImmutableList<bool>> routineRecordData)
    {
        movingWidget.UpdateData(routineRecordData);
    }

    #endregion
}