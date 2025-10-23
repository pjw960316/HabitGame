using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UniRx;
using UnityEngine;
using ScrollData = UIRoutineRecordPopup.ScrollData;

public class RoutineRecordPresenter : UIPresenterBase
{
    #region 1. Fields

    private UIRoutineRecordPopup _uiRoutineRecordPopup;
    private ImmutableSortedDictionary<string, ImmutableList<bool>> _routineRecordDictionary;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);
    }

    protected sealed override void InitializeView()
    {
        base.InitializeView();

        _uiRoutineRecordPopup = _view as UIRoutineRecordPopup;
        ExceptionHelper.CheckNullException(_uiRoutineRecordPopup, "_uiRoutineRecordPopup");
    }

    protected sealed override void InitializeModel()
    {
        // note : Manager Class를 Model로 여겼다.
        _routineRecordDictionary = _myCharacterManager.GetRoutineRecordDictionary();
    }

    public sealed override void SetView()
    {
        var routineRecordCount = _routineRecordDictionary.Count;
        var showWidgetCount = _uiRoutineRecordPopup.GetWidgetShowCount();
        var finalCount = routineRecordCount < showWidgetCount
            ? 0
            : routineRecordCount - showWidgetCount;

        if (finalCount < 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        _uiRoutineRecordPopup.SetContentsHeight(finalCount);
        _uiRoutineRecordPopup.SetWidgetData(_routineRecordDictionary);
        _uiRoutineRecordPopup.ShowTopContent();
    }

    public sealed override void BindEvent()
    {
        _uiRoutineRecordPopup.OnUpdateScrollWidget.Subscribe(UpdateWidget).AddTo(_disposable);
        _uiRoutineRecordPopup.OnCloseButtonClicked.Subscribe(_ => Close()).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    private void UpdateWidget(ScrollData scrollData)
    {
        if (IsFrontOrLastWidget(scrollData))
        {
            return;
        }

        UpdateWidgetData(scrollData);
        UpdateWidgetPosition(scrollData);
    }

    private bool IsFrontOrLastWidget(ScrollData scrollData)
    {
        var targetWidgetDate = GetTargetWidget(scrollData.IsScrollDown).Date;
        var latestDate = _routineRecordDictionary.FirstOrDefault().Key;
        var oldestDate = _routineRecordDictionary.LastOrDefault().Key;

        if (scrollData.IsScrollDown && targetWidgetDate == oldestDate)
        {
            return true;
        }

        if (!scrollData.IsScrollDown && targetWidgetDate == latestDate)
        {
            return true;
        }

        return false;
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

    private KeyValuePair<string, ImmutableList<bool>> GetBeforeDateRoutineRecord(ScrollData scrollData)
    {
        var targetWidgetDate = GetTargetWidget(scrollData.IsScrollDown).Date;
        var prev = _routineRecordDictionary.Reverse()
            .TakeWhile(kvp => kvp.Key != targetWidgetDate)
            .LastOrDefault();


        return prev;
    }

    private KeyValuePair<string, ImmutableList<bool>> GetAfterDateRoutineRecord(ScrollData scrollData)
    {
        var targetWidgetDate = GetTargetWidget(scrollData.IsScrollDown).Date;
        var next = _routineRecordDictionary
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