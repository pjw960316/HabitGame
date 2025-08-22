using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIRoutineRecordPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private GameObject _widgetPrefab;
    [SerializeField] private GameObject _contents;
    [SerializeField] private ScrollRect _routineRecordScrollRect;

    private List<UIRoutineRecordWidget> _widgetList;
    private float _widgetHeight;
    private float _currentVerticalNormalizedPosition;

    private readonly Subject<Unit> _onScrolled = new();
    public IObservable<Unit> OnScrolled => _onScrolled;

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
        _widgetHeight = _widgetPrefab.GetComponent<RectTransform>().rect.height;
        _currentVerticalNormalizedPosition = 1f;
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
        //test
        if (0.6f < _currentVerticalNormalizedPosition && _currentVerticalNormalizedPosition < 0.61f)
        {
            //test
            //observer find 필요 X
            if (_onScrolled.HasObservers)
            {
                _onScrolled?.OnNext(default);
            }
            
            //test
            //1회만 dispose
            _onScrolled?.Dispose();
        }

        _currentVerticalNormalizedPosition = _routineRecordScrollRect.verticalNormalizedPosition;
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
            new Vector2(GetComponent<RectTransform>().rect.width, _widgetHeight * widgetCount);
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
                new Vector3(0, -(index * _widgetHeight), 0);
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