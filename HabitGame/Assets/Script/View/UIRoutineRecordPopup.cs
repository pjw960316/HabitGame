using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIRoutineRecordPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private GameObject _widgetPrefab;
    [SerializeField] private GameObject _contents;
    [SerializeField] private ScrollRect _routineRecordScrollRect;

    private List<UIRoutineRecordWidget> _widgetList;

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
        var a = _routineRecordScrollRect.verticalNormalizedPosition;
        Debug.Log($"{a}");
    }


    protected override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<RoutineRecordPresenter>(this);
    }

    public void CreateRoutineRecordWidgets(int widgetCount)
    {
        for (var i = 0; i < widgetCount; i++)
        {
            //create
            var routineRecordWidget =
                Instantiate(_widgetPrefab, _contents.transform).GetComponent<UIRoutineRecordWidget>();
            _widgetList.Add(routineRecordWidget);
            
            //transform
            routineRecordWidget.transform.localPosition = new Vector3(0, i * 200, 0);
        }
    }

    //test
    public void UpdateRoutineRecordWidgets(ImmutableSortedDictionary<string, ImmutableList<bool>> routineRecordDictionary)
    {
        int index = 0;
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

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}