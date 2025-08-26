using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using ScrollData = UIRoutineRecordPopup.ScrollData;

public class RoutineRecordPresenter : PresenterBase
{
    public class RoutineRecordData
    {
        public string Date;
        public ImmutableList<bool> RoutineCheck;
    }

    #region 1. Fields

    private const int DEFAULT_WIDGET_COUNT = 5;

    private UIRoutineRecordPopup _uiRoutineRecordPopup;

    private MyCharacterManager _myCharacterManager;
    private ImmutableSortedDictionary<string, ImmutableList<bool>> _routineRecordDictionary;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    // refactor
    // Initialize()안에 막 때려 넣는 행위?
    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _myCharacterManager = MyCharacterManager.Instance;
        _uiRoutineRecordPopup = _view as UIRoutineRecordPopup;

        if (_uiRoutineRecordPopup == null)
        {
            throw new NullReferenceException("_uiRoutineRecordPopup");
        }

        // refactor
        // 이 두개의 실행흐름은 중요한데.
        // 이 실행흐름을 고려하면 2가지 일을 하더라도 합쳐야 하는가?
        //presenter logic 수행
        InitializeRoutineRecords();

        BindEvent();

        //view update
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _uiRoutineRecordPopup.OnUpdateScrollWidget.Subscribe(UpdateWidget).AddTo(_disposable);
    }


    private void InitializeRoutineRecords()
    {
        _routineRecordDictionary = _myCharacterManager.GetRoutineRecordDictionary();

        var routineRecordCount = _routineRecordDictionary.Count;
        var widgetPrefabCount = routineRecordCount < DEFAULT_WIDGET_COUNT ? routineRecordCount : DEFAULT_WIDGET_COUNT;

        //refactor
        //create 하고 무조건 widget을 세팅해야 함. 이 순서가
        _uiRoutineRecordPopup.InitializeContentsHeight(routineRecordCount);
        _uiRoutineRecordPopup.CreateRoutineRecordWidgets(widgetPrefabCount);
        _uiRoutineRecordPopup.UpdateRoutineRecordWidgets(_routineRecordDictionary);

        //test
        _uiRoutineRecordPopup.ShowTopContent();
    }

    #endregion

    #region 5. Request Methods

    private void RequestBesideData()
    {
    }

    #endregion

    #region 6. EventHandlers

    private void UpdateWidget(ScrollData scrollData)
    {
        UpdateWidgetData(scrollData);
        UpdateWidgetPosition(scrollData);
    }

    private void UpdateWidgetPosition(ScrollData scrollData)
    {
        if (scrollData.IsScrollDown)
        {
            var bottomY = _uiRoutineRecordPopup.GetBottomWidget().GetAnchoredPositionY();
            scrollData.MovingWidget.RectTransform.anchoredPosition = new Vector2(0, bottomY - scrollData.Offset);
        }
        else
        {
            var topY = _uiRoutineRecordPopup.GetTopWidget().GetAnchoredPositionY();
            scrollData.MovingWidget.RectTransform.anchoredPosition = new Vector2(0, topY + scrollData.Offset);
        }
    }

    private void UpdateWidgetData(ScrollData scrollData)
    {
        var widgetData = scrollData.IsScrollDown
            ? GetBeforeDateRoutineRecord(scrollData)
            : GetAfterDateRoutineRecord(scrollData);

        
        _uiRoutineRecordPopup.UpdateWidgetData(scrollData.MovingWidget, widgetData);
    }

    [CanBeNull]
    private KeyValuePair<string, ImmutableList<bool>> GetBeforeDateRoutineRecord(ScrollData scrollData)
    {
        var targetWidgetDate = GetTargetWidget(scrollData.IsScrollDown).Date;
        var prev = _routineRecordDictionary
            .TakeWhile(kvp => kvp.Key != targetWidgetDate)
            .LastOrDefault();

        return prev;
    }

    [CanBeNull]
    private KeyValuePair<string, ImmutableList<bool>> GetAfterDateRoutineRecord(ScrollData scrollData)
    {
        var targetWidgetDate = GetTargetWidget(scrollData.IsScrollDown).Date;
        var next = _routineRecordDictionary.Reverse()
            .TakeWhile(kvp => kvp.Key != targetWidgetDate)
            .LastOrDefault();

        return next;
    }

    private UIRoutineRecordWidget GetTargetWidget(bool isScrollDown)
    {
        if (isScrollDown)
        {
            return _uiRoutineRecordPopup.GetBottomWidget();
        }

        return _uiRoutineRecordPopup.GetTopWidget();
    }

    #endregion
}