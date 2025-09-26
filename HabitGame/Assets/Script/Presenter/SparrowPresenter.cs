using System;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class SparrowPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private const int FULL_ROTATION = 360;
    private const float COLLIDED_ROCK_ANIMATION_CHANGE_SECOND = 1f;
    private const int DIRECTION_CHANGE_INTERVAL_SECOND_MAX = 10;
    private const int DIRECTION_CHANGE_INTERVAL_UPDATE_PERIOD_SECOND = 5;
    private const int EAT_SECOND = 15;

    private FieldObjectSparrow _fieldObjectSparrow;
    private SparrowData _sparrowData;
    private double _directionChangeIntervalSecond;
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

        BindWalkRandomEvent();
    }

    // refactor
    // 네이밍 너무 어려움
    private void BindWalkRandomEvent()
    {
        // note : 방향 전환 하는 Observable의 타이머 값을 랜덤 하게
        Observable.Interval(TimeSpan.FromSeconds(DIRECTION_CHANGE_INTERVAL_UPDATE_PERIOD_SECOND)).Subscribe(_ =>
        {
            _directionChangeIntervalSecond = _randomMaker.NextDouble() * DIRECTION_CHANGE_INTERVAL_SECOND_MAX;
        }).AddTo(_disposable);

        // note : 참새의 성향에 따라 방향전환의 빈도를 Observable로 관리
        Observable.Interval(TimeSpan.FromSeconds(_impatienceLevel)).Subscribe(_ =>
        {
            ChangeDirectionRandomlyIfWalk(_currentSparrowState);
        }).AddTo(_disposable);
    }

    #endregion

    #region 4-1. EventHandlers - Normal

    public void OnChangeSparrowState(ESparrowState changedState)
    {
        _fieldObjectSparrow.ChangeAnimation((int)changedState);

        if (changedState == ESparrowState.WALK)
        {
            _fieldObjectSparrow.ChangeSparrowDefaultSpeed();
        }
    }

    #endregion

    # region 4-2. EventHandlers - Collision

    private void OnCollision(Collision collision)
    {
        var fieldObjectBase = collision.gameObject.GetComponentInParent<FieldObjectBase>();
        ExceptionHelper.CheckNullException(fieldObjectBase, "fieldObjectBase script X");

        Debug.Log($"{_fieldObjectSparrow.name}이 {collision.gameObject.name}이랑 부딪혔다.");
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
        }
    }

    private void OnCollideWithRock()
    {
        Debug.Log("ROCK");
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);

        // todo
        // 의도 되지 않은 타이밍에 콜 되는 거 막기
        Observable.Timer(TimeSpan.FromSeconds(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND)).Subscribe(_ =>
        {
            _fieldObjectSparrow.ChangeSparrowPath(FULL_ROTATION / 2);

            _sparrowData.ChangeSparrowState(ESparrowState.WALK);
        }).AddTo(_disposable);
    }

    private void OnCollideWithEatableEnvironment()
    {
        Debug.Log("MushRoom | Flower");
        _sparrowData.ChangeSparrowState(ESparrowState.EAT);
        
        Observable.Timer(TimeSpan.FromSeconds(EAT_SECOND)).Subscribe(_ =>
        {
            _sparrowData.ChangeSparrowState(ESparrowState.WALK);
        }).AddTo(_disposable);
    }

    private void OnCollideWithOtherSparrow()
    {
        Debug.Log("OtherSparrow");
        
        _fieldObjectSparrow.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(ESparrowState.ATTACK);

        Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        {
            _sparrowData.ChangeSparrowState(ESparrowState.WALK);
        }).AddTo(_disposable);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    private void ChangeDirectionRandomlyIfWalk(ESparrowState changedState)
    {
        Observable.Timer(TimeSpan.FromSeconds(_directionChangeIntervalSecond)).Subscribe(_ =>
        {
            if (changedState != ESparrowState.WALK)
            {
                return;
            }

            Debug.Log($"{_fieldObjectSparrow.name}  :  {_directionChangeIntervalSecond}");

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