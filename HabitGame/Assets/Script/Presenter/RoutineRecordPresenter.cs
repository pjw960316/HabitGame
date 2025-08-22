// refactor
// RoutineCheckPresenter와 함께 하려 했으나, presenter가 2개의 view를 책임지는 건 아니라 판단.
// 그래서 두 개를 일단 분리해서 구현
// 하지만 왠지 두 개의 공통 기능이 생길 것 같음 -> 상위로 묶는 방식
// 이거에 대한 판단이 아직 서지 않는다.

using System;
using System.Collections.Immutable;
using UniRx;
using UnityEngine;

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
        _uiRoutineRecordPopup.OnScrolled.Subscribe(_ => Test()).AddTo(_disposable);
    }

    private void Test()
    {
        var bottomY = _uiRoutineRecordPopup.GetBottomWidget().GetAnchoredPositionY();
        
        _uiRoutineRecordPopup.GetTopWidget().RectTransform.anchoredPosition = new Vector2(0, bottomY - 200f);
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

    private void UpdateRoutineRecords()
    {
    }

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    //

    #endregion
}