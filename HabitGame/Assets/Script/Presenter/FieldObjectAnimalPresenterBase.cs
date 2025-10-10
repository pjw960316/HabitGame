using System;
using UniRx;
using UnityEngine;
using Random = System.Random;


// note
// 모든 동물은 걷고, 랜덤으로 방향을 돌린다.
public abstract class FieldObjectAnimalPresenterBase : FieldObjectPresenterBase
{
    #region 1. Fields

    private const int FULL_ROTATION = 360;
    protected const int HALF_ROTATION = 180;
    protected const int QUARTER_ROTATION = 90;

    protected const float ANIMAL_FIGHT_SECOND = 2f;
    protected const float COLLIDED_ROCK_ANIMATION_CHANGE_SECOND = 1f;

    private const int DIRECTION_CHANGE_INTERVAL_SECOND_MAX = 10;
    private const int DIRECTION_CHANGE_INTERVAL_UPDATE_PERIOD_SECOND = 5;


    protected EAnimalState _currentAnimalState;
    private FieldObjectAnimalBase _fieldObjectAnimal;
    protected FieldObjectAnimalData _animalData;

    private int _directionChangeIntervalSecond;
    private int _impatienceLevel;

    private readonly CompositeDisposable _animalRandomPathDisposable = new();
    private readonly Random _randomMaker = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public override void Initialize(IView view)
    {
        base.Initialize(view);

        // view
        if (_view is FieldObjectAnimalBase animal)
        {
            _fieldObjectAnimal = animal;
        }

        ExceptionHelper.CheckNullException(_fieldObjectAnimal, "_fieldObjectAnimal is null");

        // model
        if (_model is FieldObjectAnimalData animalData)
        {
            _animalData = animalData;
        }

        ExceptionHelper.CheckNullException(_animalData, "_sparrowData is null");

        _currentAnimalState = _animalData.GetAnimalState();

        // note : 이 값이 낮으면 성격이 급하다 -> 방향 전환을 자주한다.
        _impatienceLevel =
            _randomMaker.Next(DIRECTION_CHANGE_INTERVAL_SECOND_MAX / 2, DIRECTION_CHANGE_INTERVAL_SECOND_MAX);
    }

    protected override void BindEvent()
    {
        base.BindEvent();

        _fieldObjectAnimal.OnCollision.Subscribe(OnCollision).AddTo(_disposable);
        _animalData.OnAnimalStateChanged.Subscribe(OnChangeAnimalState).AddTo(_disposable);

        // 걷기 방향은 모든 동물에서 동작시킨다.
        BindDirectionChangeInterval();
        BindChangeRandomDirection();
    }

    private void BindDirectionChangeInterval()
    {
        Observable.Interval(TimeSpan.FromSeconds(DIRECTION_CHANGE_INTERVAL_UPDATE_PERIOD_SECOND))
            .Subscribe(_ =>
            {
                _directionChangeIntervalSecond = _randomMaker.Next(0, DIRECTION_CHANGE_INTERVAL_SECOND_MAX);
            }).AddTo(_disposable);
    }

    private void BindChangeRandomDirection()
    {
        Observable.Interval(TimeSpan.FromSeconds(_impatienceLevel))
            .Subscribe(_ => { ChangeDirectionRandomlyIfWalk(); }).AddTo(_disposable);
    }

    #endregion

    #region 4-1. EventHandlers - Normal

    // note : AnimalData에서 state를 바꾸면 ReactiveProperty로 인해 콜이 된다.
    public void OnChangeAnimalState(EAnimalState changedState)
    {
        var predicate = _animalData.CanChangeState;
        if (!predicate)
        {
            return;
        }
        
        _currentAnimalState = changedState;
        _fieldObjectAnimal.ChangeAnimation((int)_currentAnimalState);

        // Log
        Debug.Log($"{_fieldObjectAnimal.name}의 현재 상태 : {_currentAnimalState}");

        if (_currentAnimalState == EAnimalState.WALK)
        {
            _fieldObjectAnimal.ChangeAnimalDefaultSpeed();
        }
    }

    #endregion

    # region 4-2. EventHandlers - Collision

    protected void OnCollision(Collision collision)
    {
        var fieldObjectBase = collision.gameObject.GetComponentInParent<FieldObjectBase>();
        ExceptionHelper.CheckNullException(fieldObjectBase, "fieldObjectBase script X");

        _fieldObjectAnimal.StopAnimalMoving();

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
            case FieldObjectDeer:
                OnCollideWithAnimal();
                break;
            case FieldObjectTree:
                OnCollideWithTree();
                break;
        }
    }

    protected virtual void OnCollideWithRock()
    {
        _animalData.ChangeAnimalState(EAnimalState.IDLE);
        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    protected virtual void OnCollideWithEatableEnvironment()
    {
        _animalData.ChangeAnimalState(EAnimalState.IDLE);
        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    protected void OnCollideWithAnimal()
    {
        _fieldObjectAnimal.RotateToFaceCollisionObject();

        _animalData.ChangeAnimalState(EAnimalState.ATTACK);
        ChangeToWalkStateAfterDelay(ANIMAL_FIGHT_SECOND, QUARTER_ROTATION);
    }

    protected virtual void OnCollideWithTree()
    {
        _animalData.ChangeAnimalState(EAnimalState.IDLE);
        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected void ChangeToWalkStateAfterDelay(float delaySeconds, int sparrowRotationDegree)
    {
        Observable.Timer(TimeSpan.FromSeconds(delaySeconds)).Subscribe(_ =>
        {
            _fieldObjectAnimal.ChangeAnimalPath(sparrowRotationDegree);
            _animalData.ChangeAnimalState(EAnimalState.WALK);
        }).AddTo(_disposable);
    }

    private void ChangeDirectionRandomlyIfWalk()
    {
        Observable.Timer(TimeSpan.FromSeconds(_directionChangeIntervalSecond)).Subscribe(_ =>
        {
            if (_currentAnimalState != EAnimalState.WALK)
            {
                return;
            }

            var randDegree = _randomMaker.Next(0, FULL_ROTATION);
            _fieldObjectAnimal.ChangeAnimalPath(randDegree);
        }).AddTo(_animalRandomPathDisposable);
    }

    protected sealed override void DisposeCompositeDisposables()
    {
        base.DisposeCompositeDisposables();

        _animalRandomPathDisposable?.Dispose();
    }

    #endregion
}