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
        public readonly UIRoutineRecordWidget Widget;

        public ScrollData(float offset, bool isScrollDown, UIRoutineRecordWidget widget)
        {
            Offset = offset;
            IsScrollDown = isScrollDown;
            Widget = widget;
        }
    }
    
    #region 1. Fields
    
    [SerializeField] private GameObject _widgetPrefab;
    [SerializeField] private GameObject _viewPort;
    [SerializeField] private GameObject _contents;
    [SerializeField] private ScrollRect _routineRecordScrollRect;

    private const float WIDGET_SCROLL_DOWN_OFFSET = 2f;
    private const float WIDGET_SCROLL_UP_OFFSET = 1.5f;
    
    private List<UIRoutineRecordWidget> _widgetList;
    private float _widgetOffsetHeight;
    private float _viewPortWorldPosY;
    private float _currentVerticalNormalizedPosition;

    private readonly Subject<ScrollData> _onUpdateScrollWidget = new();
    public IObservable<ScrollData> OnUpdateScrollWidget => _onUpdateScrollWidget;

    #endregion

    #region 2. Properties

    // default

    #endregion
    
    #region 3. Constructor

    public override void OnAwake()
    {
        base.OnAwake();

        PreInitialize();

        CreatePresenterByManager();

        Initialize();

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void PreInitialize()
    {
        _widgetList = new List<UIRoutineRecordWidget>();
        _widgetOffsetHeight = _widgetPrefab.GetComponent<RectTransform>().rect.height;
        
        _currentVerticalNormalizedPosition = 1f;
        _viewPortWorldPosY = _viewPort.transform.position.y;
    }

    private void Initialize()
    {
    }

    private void BindEvent()
    {
        _routineRecordScrollRect.onValueChanged.AddListener(_ => OnScroll());
    }

    private void OnScroll()
    {
        UpdateWidgetPositionIfNeeded();
        
        UpdateCurrentVerticalNormalizedPosition();
    }

    private void UpdateWidgetPositionIfNeeded()
    {
        var isScrollDown = _routineRecordScrollRect.verticalNormalizedPosition < _currentVerticalNormalizedPosition;
        var targetWidget = isScrollDown ? GetTopWidget() : GetBottomWidget();
        var data = new ScrollData(_widgetOffsetHeight, isScrollDown, targetWidget);
        
        if (isScrollDown)
        {
            var shouldScrollUpdate = _viewPortWorldPosY + _widgetOffsetHeight * WIDGET_SCROLL_DOWN_OFFSET <
                                     targetWidget.WorldPosY;
            if (shouldScrollUpdate)
            {
                _onUpdateScrollWidget?.OnNext(data);
            }
        }
        else
        {
            var shouldScrollUpdate = targetWidget.WorldPosY <
                                     _viewPortWorldPosY - _widgetOffsetHeight * WIDGET_SCROLL_UP_OFFSET;
            if (shouldScrollUpdate)
            {
                _onUpdateScrollWidget?.OnNext(data);
            }
        }
    }

    private void UpdateCurrentVerticalNormalizedPosition()
    {
        _currentVerticalNormalizedPosition = _routineRecordScrollRect.verticalNormalizedPosition;
    }
    
    public UIRoutineRecordWidget GetTopWidget()
    {
        return GetNestedWidget(isBottom : false);
    }

    public UIRoutineRecordWidget GetBottomWidget()
    {
        return GetNestedWidget(isBottom : true);
    }

    // Note
    // GetTopWidget , GetBottomWidget Nested Method
    private UIRoutineRecordWidget GetNestedWidget(bool isBottom)
    {
        var currentY = _widgetList[0].GetAnchoredPositionY();
        var targetWidget = _widgetList[0];
        
        foreach (var widget in _widgetList)
        {
            var widgetAnchoredPositionY = widget.GetAnchoredPositionY();
            if ((isBottom && currentY > widgetAnchoredPositionY) || (!isBottom && currentY < widgetAnchoredPositionY))
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
            new Vector2(GetComponent<RectTransform>().rect.width, _widgetOffsetHeight * widgetCount);
    }

    protected override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<RoutineRecordPresenter>(this);
    }

    //test
    public void CreateRoutineRecordWidgets(int widgetCount)
    {
        for (var index = 0; index < widgetCount; index++)
        {
            //create
            var routineRecordWidget =
                Instantiate(_widgetPrefab, _contents.transform).GetComponent<UIRoutineRecordWidget>();
            _widgetList.Add(routineRecordWidget);

            //transform
            routineRecordWidget.RectTransform.anchoredPosition =
                new Vector3(0, -(index * _widgetOffsetHeight), 0);
        }
    }

    //test
    public void UpdateRoutineRecordWidgets(
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
        _routineRecordScrollRect.verticalNormalizedPosition = 1f;
    }

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}