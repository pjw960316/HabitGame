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
        public float Offset;
        public bool IsScrollDown;
        public UIRoutineRecordWidget Widget;

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

    private const float DOUBLE = 2f;
    
    private List<UIRoutineRecordWidget> _widgetList;
    private float _widgetOffsetHeight;
    private float _viewPortWorldPosY;
    private float _currentVerticalNormalizedPosition;

    private readonly Subject<ScrollData> _onScrolled = new();
    public IObservable<ScrollData> OnScrolled => _onScrolled;

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
        Debug.Log($"{_viewPortWorldPosY}");
    }

    private void BindEvent()
    {
        _routineRecordScrollRect.onValueChanged.AddListener(_ => OnScroll());
    }

    private void OnScroll()
    {
        // 내릴 때
        if (isScrollDown())
        {
            var targetWidget = GetTopWidget();
            ScrollData data = new ScrollData(200, true, targetWidget);
            
            if (_viewPortWorldPosY + _widgetOffsetHeight * DOUBLE < targetWidget.WorldPosY)
            {
                _onScrolled?.OnNext(data);
            }
        }
        
        // 올릴 때
        else
        {
            var targetWidget = GetBottomWidget();
            ScrollData data = new ScrollData(200, false, targetWidget);
            
            if (targetWidget.WorldPosY < _viewPortWorldPosY - _widgetOffsetHeight * 1.5f)
            {
                _onScrolled?.OnNext(data);
            }
        }

        _currentVerticalNormalizedPosition = _routineRecordScrollRect.verticalNormalizedPosition;
    }

    private bool isScrollDown()
    {
        return _routineRecordScrollRect.verticalNormalizedPosition < _currentVerticalNormalizedPosition;
    }
    
    //refactor
    //2개 합치기
    public UIRoutineRecordWidget GetTopWidget()
    {
        var currentY = _widgetList[0].GetAnchoredPositionY();
        var topWidget = _widgetList[0];
        foreach (var widget in _widgetList)
        {
            var widgetAnchoredPositionY = widget.GetAnchoredPositionY();
            if (currentY < widgetAnchoredPositionY)
            {
                currentY = widgetAnchoredPositionY;
                topWidget = widget;
            }
        }

        return topWidget;
    }

    //refactor
    //2개 합치기
    public UIRoutineRecordWidget GetBottomWidget()
    {
        var currentY = _widgetList[0].GetAnchoredPositionY();
        var bottomWidget = _widgetList[0];
        foreach (var widget in _widgetList)
        {
            var widgetAnchoredPositionY = widget.GetAnchoredPositionY();
            if (currentY > widgetAnchoredPositionY)
            {
                currentY = widgetAnchoredPositionY;
                bottomWidget = widget;
            }
        }

        return bottomWidget;
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