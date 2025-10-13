using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIRoutineRecordPopup : UIPopupBase
{
    public struct ScrollData
    {
        public readonly float Offset;
        public readonly bool IsScrollDown;
        public readonly UIRoutineRecordWidget MovingWidget; // note : 보이지 않아서 움직일 widget 

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
    
    [SerializeField] private GameObject _widgetPrefab;
    [SerializeField] private GameObject _viewPort;
    [SerializeField] private GameObject _contents;
    [SerializeField] private ScrollRect _routineRecordScrollRect;
    [SerializeField] private UIButtonBase _closeBtn;

    private List<UIRoutineRecordWidget> _widgetList;
    private float _widgetOffsetHeight;
    private float _viewPortWorldPosY;
    private float _currentVerticalNormalizedPosition;

    private readonly Subject<ScrollData> _onUpdateScrollWidget = new();
    private readonly Subject<Unit> _onCloseButtonClicked = new Subject<Unit>();
    
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
        
        _widgetList = new List<UIRoutineRecordWidget>();
        _widgetOffsetHeight = _widgetPrefab.GetComponent<RectTransform>().rect.height;

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

    protected override void BindEvent()
    {
        _routineRecordScrollRect.onValueChanged.AddListener(_ => OnScroll());
        
        _closeBtn.OnClick.AddListener(() =>
        {
            _onCloseButtonClicked.OnNext(default);
        });
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
        var isScrollDown = _routineRecordScrollRect.verticalNormalizedPosition < _currentVerticalNormalizedPosition;
        var movingWidget = isScrollDown ? GetTopWidget() : GetBottomWidget();

        if (isScrollDown)
        {
            var shouldScrollUpdate = movingWidget.WorldPosY >
                                     _viewPortWorldPosY + _widgetOffsetHeight * WIDGET_SCROLL_DOWN_OFFSET;
            if (shouldScrollUpdate)
            {
                var data = new ScrollData(_widgetOffsetHeight, isScrollDown, movingWidget);
                _onUpdateScrollWidget?.OnNext(data);
            }
        }
        else
        {
            var shouldScrollUpdate = movingWidget.WorldPosY <
                                     _viewPortWorldPosY - _widgetOffsetHeight * WIDGET_SCROLL_UP_OFFSET;
            if (shouldScrollUpdate)
            {
                var data = new ScrollData(_widgetOffsetHeight, isScrollDown, movingWidget);
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
            new Vector2(GetComponent<RectTransform>().rect.width, _widgetOffsetHeight * widgetCount);
    }

    public void CreateRoutineRecordWidgetPrefabs(int widgetCount)
    {
        for (var index = 0; index < widgetCount; index++)
        {
            var routineRecordWidget =
                Instantiate(_widgetPrefab, _contents.transform).GetComponent<UIRoutineRecordWidget>();
            
            
            //test
            var contentsRect = _contents.GetComponent<RectTransform>().rect;
            var viewportRect = _contents.GetComponent<RectTransform>().rect;
            
            routineRecordWidget.gameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2(contentsRect.width/2f, 200f); // 200f 수정.
            
            
            Debug.Log($"{_contents.GetComponent<RectTransform>().rect.x} {_contents.GetComponent<RectTransform>().rect.width}");
            Debug.Log($"{_contents.GetComponent<RectTransform>().anchoredPosition.x} {_contents.GetComponent<RectTransform>().anchoredPosition.y}");
            
            _widgetList.Add(routineRecordWidget);

            routineRecordWidget.RectTransform.anchoredPosition =
                new Vector3(0, -(index * _widgetOffsetHeight), 0);
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
        _routineRecordScrollRect.verticalNormalizedPosition = 1f;
    }

    public void UpdateWidgetData(UIRoutineRecordWidget movingWidget, KeyValuePair<string, ImmutableList<bool>> routineRecordData)
    {
        movingWidget.UpdateData(routineRecordData);
    }

    #endregion
}