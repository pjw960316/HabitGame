using System;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class FieldObjectSparrowPresenter : FieldObjectAnimalPresenterBase
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
    private int _directionChangeIntervalSecond;
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
        
        
        _currentSparrowState = _sparrowData.GetSparrowState();

        

        BindEvent();
    }


    protected sealed override void BindEvent()
    {
        base.BindEvent();
        
        _sparrowData.OnSparrowStateChanged.Subscribe(OnChangeSparrowState).AddTo(_disposable);
        _myCharacterManager.OnUpdateRoutineSuccess.Subscribe(OnChangeSparrowSpinState).AddTo(_disposable);

        //BindWalkRandomEvent();
    }

    // refactor
    // 네이밍 너무 어려움
    /*private void BindWalkRandomEvent()
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
    }*/

    #endregion

    #region 4-1. EventHandlers - Normal

    // note : SparrowData에서 state를 바꾸면 ReactiveProperty로 인해 콜이 된다.
    public void OnChangeSparrowState(EAnimalState changedState)
    {
        _currentSparrowState = changedState;
        _fieldObjectSparrow.ChangeAnimation((int)_currentSparrowState);

        // log
        // Debug.Log($"{_fieldObjectSparrow.name}'s State Change -> {_currentSparrowState}");

        if (_currentSparrowState == EAnimalState.WALK)
        {
            _fieldObjectSparrow.ChangeAnimalDefaultSpeed();
        }
    }

    private void OnChangeSparrowSpinState(Unit _)
    {
        _fieldObjectSparrow.ChangeAnimalSpeedZero();
        _sparrowData.ChangeSparrowState(EAnimalState.SPIN);
        
        ChangeToWalkStateAfterDelay(DANCE_SECOND, QUARTER_ROTATION);
    }

    #endregion

    # region 4-2. EventHandlers - Collision

    protected sealed override void OnCollision(Collision collision)
    {
        var fieldObjectBase = collision.gameObject.GetComponentInParent<FieldObjectBase>();
        ExceptionHelper.CheckNullException(fieldObjectBase, "fieldObjectBase script X");
        
        _fieldObjectSparrow.StopAnimalMoving();

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
        _sparrowData.ChangeSparrowState(EAnimalState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    private void OnCollideWithEatableEnvironment()
    {
        _fieldObjectSparrow.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(EAnimalState.EAT);
        ChangeToWalkStateAfterDelay(EAT_SECOND, HALF_ROTATION);
    }

    private void OnCollideWithOtherSparrow()
    {
        _fieldObjectSparrow.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(EAnimalState.ATTACK);

        ChangeToWalkStateAfterDelay(1f, QUARTER_ROTATION);
    }

    // test
    //일단 rock
    private void OnCollideWithTree()
    {
        _sparrowData.ChangeSparrowState(EAnimalState.FLY);

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
            _fieldObjectSparrow.ChangeAnimalPath(sparrowRotationDegree);
            
            _sparrowData.ChangeSparrowState(EAnimalState.WALK);
        }).AddTo(_disposable);
    }
    
    private void ChangeDirectionRandomlyIfWalk()
    {
        Observable.Timer(TimeSpan.FromSeconds(_directionChangeIntervalSecond)).Subscribe(_ =>
        {
            if (_currentSparrowState != EAnimalState.WALK)
            {
                return;
            }

            var randDegree = _randomMaker.Next(0, FULL_ROTATION);
            _fieldObjectSparrow.ChangeAnimalPath(randDegree);
        }).AddTo(_sparrowRandomPathDisposable);
    }

    /*protected sealed override void DisposeCompositeDisposables()
    {
        base.DisposeCompositeDisposables();

        _sparrowRandomPathDisposable?.Dispose();
    }*/

    #endregion
}