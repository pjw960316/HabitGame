using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UniRx;

public class RoutineCheckPresenter : UIPresenterBase
{
    #region 1. Fields

    private UIRoutineCheckPopup _uiRoutineCheckPopup;

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

        _uiRoutineCheckPopup = _view as UIRoutineCheckPopup;
        ExceptionHelper.CheckNullException(_uiRoutineCheckPopup, "_uiRoutineCheckPopup");
    }

    protected sealed override void InitializeModel()
    {
        // note : model does not need
    }

    public sealed override void SetView()
    {
        UpdateDateTextPerSecond();
        SetRoutineCheckToggleWidget(DateTime.Now);
    }

    public sealed override void BindEvent()
    {
        _uiRoutineCheckPopup.OnConfirmed.Subscribe(_ => HandleToggleEvent()).AddTo(_disposable);
    }

    #endregion

    #region 4. Methods

    private void UpdateDateTextPerSecond()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => { _uiRoutineCheckPopup.UpdateDateText(DateTime.Now); });
    }

    private void SetRoutineCheckToggleWidget(DateTime dateTime)
    {
        var todaySuccessfulRoutineIndex = RequestGetTodaySuccessfulRoutineIndex(dateTime);

        // note
        // null이면 해당 루틴이 없는 것 이므로
        // Toggle을 그냥 냅두면 된다.
        if (todaySuccessfulRoutineIndex == null)
        {
            return;
        }

        _uiRoutineCheckPopup.SetToggle(todaySuccessfulRoutineIndex);
    }

    #endregion

    #region 5. Request Methods

    private async UniTaskVoid RequestUpdateRoutineRecordAsync(List<int> todaySuccessfulRoutineIndex, DateTime dateTime)
    {
        // note : 서버 시간 딜레이 이후에 꺼지므로, 그 때 접근 못하도록 (cancellation Token 대신)
        _uiRoutineCheckPopup.BlockConfirmButton();

        var serverResult = await _serverManager.RequestServerValidation();

        if (serverResult == EServerResult.SUCCESS)
        {
            _myCharacterManager.UpdateRoutineRecord(todaySuccessfulRoutineIndex, dateTime);

            Close();

            RequestShowToast();
        }
        else
        {
            _uiRoutineCheckPopup.UnBlockConfirmButton();
        }
    }

    [CanBeNull]
    private List<int> RequestGetTodaySuccessfulRoutineIndex(DateTime dateTime)
    {
        return _myCharacterManager.GetTodaySuccessfulRoutineIndex(dateTime);
    }

    private void RequestShowToast()
    {
        _uiToastManager.ShowToast(EToastStringKey.ERoutineCheckConfirm,
            _myCharacterManager.GetMonthlyRoutineSuccessMoney().ToString("N0"));
    }

    #endregion

    #region 6. EventHandlers

    private void HandleToggleEvent()
    {
        var toggleList = _uiRoutineCheckPopup.GetToggleList();
        var todaySuccessfulRoutineIndex = new List<int>();
        var toggleListCount = toggleList.Count;

        for (var index = 0; index < toggleListCount; index++)
        {
            var toggle = toggleList[index];
            var isToggleChecked = toggle.GetToggle().isOn;

            if (isToggleChecked)
            {
                todaySuccessfulRoutineIndex.Add(index);
            }
        }

        RequestUpdateRoutineRecordAsync(todaySuccessfulRoutineIndex, DateTime.Now).Forget();
    }

    #endregion
}