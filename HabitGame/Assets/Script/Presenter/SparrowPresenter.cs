using System;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class SparrowPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private const int FULL_ROTATION = 360;
    private const int HALF_ROTATION = 180;
    private const int QUARTER_ROTATION = 90;
    
    private const float COLLIDED_ROCK_ANIMATION_CHANGE_SECOND = 1f;
    private const int DIRECTION_CHANGE_INTERVAL_SECOND_MAX = 10;
    private const int DIRECTION_CHANGE_INTERVAL_UPDATE_PERIOD_SECOND = 5;
    private const int EAT_SECOND = 15;
    private const int DANCE_SECOND = 5;

    private FieldObjectSparrow _fieldObjectSparrow;
    private SparrowData _sparrowData;
    private int _directionChangeIntervalSecond;
    private ESparrowState _currentSparrowState;
    private int _impatienceLevel;

    private readonly CompositeDisposable _sparrowRandomPathDisposable = new();
    private readonly Random _randomMaker = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        // view
        if (_view is FieldObjectSparrow sparrow)
        {
            _fieldObjectSparrow = sparrow;
        }

        ExceptionHelper.CheckNullException(_fieldObjectSparrow, "_fieldObjectSparrow is null");

        if (_model is SparrowData sparrowData)
        {
            _sparrowData = sparrowData;
        }

        ExceptionHelper.CheckNullException(_sparrowData, "_sparrowData is null");

        _currentSparrowState = _sparrowData.GetSparrowState();

        // note
        // 이 값이 낮으면 성격이 급하다 -> 방향 전환을 자주한다.
        _impatienceLevel =
            _randomMaker.Next(DIRECTION_CHANGE_INTERVAL_SECOND_MAX / 2, DIRECTION_CHANGE_INTERVAL_SECOND_MAX);

        BindEvent();
    }


    protected sealed override void BindEvent()
    {
        base.BindEvent();

        _fieldObjectSparrow.OnCollision.Subscribe(OnCollision).AddTo(_disposable);
        _sparrowData.OnSparrowStateChanged.Subscribe(OnChangeSparrowState).AddTo(_disposable);
        _myCharacterManager.OnUpdateRoutineSuccess.Subscribe(OnChangeSparrowSpinState).AddTo(_disposable);

        BindWalkRandomEvent();
    }

    // refactor
    // 네이밍 너무 어려움
    private void BindWalkRandomEvent()
    {
        // note : 방향 전환 하는 Observable의 타이머 값을 랜덤 하게
        Observable.Interval(TimeSpan.FromSeconds(DIRECTION_CHANGE_INTERVAL_UPDATE_PERIOD_SECOND))
            .Subscribe(_ =>
            {
                _directionChangeIntervalSecond = _randomMaker.Next(0, DIRECTION_CHANGE_INTERVAL_SECOND_MAX);
            }).AddTo(_disposable);

        // note : 참새의 성향에 따라 방향전환의 빈도를 Observable로 관리
        Observable.Interval(TimeSpan.FromSeconds(_impatienceLevel))
            .Subscribe(_ => { ChangeDirectionRandomlyIfWalk(); })
            .AddTo(_disposable);
    }

    #endregion

    #region 4-1. EventHandlers - Normal

    // note : SparrowData에서 state를 바꾸면 ReactiveProperty로 인해 콜이 된다.
    public void OnChangeSparrowState(ESparrowState changedState)
    {
        _currentSparrowState = changedState;
        _fieldObjectSparrow.ChangeAnimation((int)_currentSparrowState);

        Debug.Log($"{_fieldObjectSparrow.name}'s State Change -> {_currentSparrowState}");

        if (_currentSparrowState == ESparrowState.WALK)
        {
            _fieldObjectSparrow.ChangeSparrowDefaultSpeed();
        }
    }

    private void OnChangeSparrowSpinState(Unit _)
    {
        _fieldObjectSparrow.ChangeSparrowSpeedZero();
        _sparrowData.ChangeSparrowState(ESparrowState.SPIN);
        
        ChangeToWalkStateAfterDelay(DANCE_SECOND, QUARTER_ROTATION);
    }

    #endregion

    # region 4-2. EventHandlers - Collision

    private void OnCollision(Collision collision)
    {
        var fieldObjectBase = collision.gameObject.GetComponentInParent<FieldObjectBase>();
        ExceptionHelper.CheckNullException(fieldObjectBase, "fieldObjectBase script X");

        Debug.Log($"{_fieldObjectSparrow.name}가 {fieldObjectBase.EFieldObjectKey}와 부딪혔다");
        
        _fieldObjectSparrow.StopSparrowMoving();

        switch (fieldObjectBase)
        {
            case FieldObjectRock:
                OnCollideWithRock();
                break;
            case FieldObjectMushroom:
            case FieldObjectFlower:
                OnCollideWithEatableEnvironment();
                break;
            case FieldObjectSparrow:
                OnCollideWithOtherSparrow();
                break;
            case FieldObjectTree:
                OnCollideWithTree();
                break;
        }
    }

    private void OnCollideWithRock()
    {
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    private void OnCollideWithEatableEnvironment()
    {
        _fieldObjectSparrow.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(ESparrowState.EAT);
        ChangeToWalkStateAfterDelay(EAT_SECOND, HALF_ROTATION);
    }

    private void OnCollideWithOtherSparrow()
    {
        _fieldObjectSparrow.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(ESparrowState.ATTACK);

        ChangeToWalkStateAfterDelay(1f, QUARTER_ROTATION);
    }

    // test
    //일단 rock
    private void OnCollideWithTree()
    {
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    private void ChangeToWalkStateAfterDelay(float delaySeconds, int sparrowRotationDegree)
    {
        Observable.Timer(TimeSpan.FromSeconds(delaySeconds)).Subscribe(_ =>
        {
            _fieldObjectSparrow.ChangeSparrowPath(sparrowRotationDegree);
            
            _sparrowData.ChangeSparrowState(ESparrowState.WALK);
        }).AddTo(_disposable);
    }
    
    private void ChangeDirectionRandomlyIfWalk()
    {
        Observable.Timer(TimeSpan.FromSeconds(_directionChangeIntervalSecond)).Subscribe(_ =>
        {
            if (_currentSparrowState != ESparrowState.WALK)
            {
                return;
            }

            var randDegree = _randomMaker.Next(0, FULL_ROTATION);
            _fieldObjectSparrow.ChangeSparrowPath(randDegree);
        }).AddTo(_sparrowRandomPathDisposable);
    }

    protected sealed override void DisposeCompositeDisposables()
    {
        base.DisposeCompositeDisposables();

        _sparrowRandomPathDisposable?.Dispose();
    }

    #endregion
}