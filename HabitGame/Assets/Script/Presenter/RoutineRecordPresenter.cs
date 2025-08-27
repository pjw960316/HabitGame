using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UniRx;
using UnityEngine;
using ScrollData = UIRoutineRecordPopup.ScrollData;

public class RoutineRecordPresenter : PresenterBase
{
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

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _myCharacterManager = MyCharacterManager.Instance;
        _routineRecordDictionary = _myCharacterManager.GetRoutineRecordDictionary();

        _uiRoutineRecordPopup = _view as UIRoutineRecordPopup;
        ExceptionHelper.CheckNullException(_uiRoutineRecordPopup, "_uiRoutineRecordPopup");

        SetView();

        BindEvent();
    }

    protected sealed override void SetView()
    {
        var routineRecordCount = _routineRecordDictionary.Count;
        var widgetPrefabCount = routineRecordCount < DEFAULT_WIDGET_COUNT ? routineRecordCount : DEFAULT_WIDGET_COUNT;

        _uiRoutineRecordPopup.InitializeContentsHeight(routineRecordCount);
        _uiRoutineRecordPopup.CreateRoutineRecordWidgetPrefabs(widgetPrefabCount);
        _uiRoutineRecordPopup.InitializeRoutineRecordWidgets(_routineRecordDictionary);
        _uiRoutineRecordPopup.ShowTopContent();
    }

    private void BindEvent()
    {
        _uiRoutineRecordPopup.OnUpdateScrollWidget.Subscribe(UpdateWidget).AddTo(_disposable);
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