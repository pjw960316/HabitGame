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
    private const int HALF_ROTATION = 180;
    private const int QUARTER_ROTATION = 90;

    private const float COLLIDED_ROCK_ANIMATION_CHANGE_SECOND = 1f;
    private const int DIRECTION_CHANGE_INTERVAL_SECOND_MAX = 10;
    private const int DIRECTION_CHANGE_INTERVAL_UPDATE_PERIOD_SECOND = 5;
    private const int EAT_SECOND = 15;
    private const int DANCE_SECOND = 5;

    protected EAnimalState _currentAnimalState;
    private FieldObjectAnimalBase _fieldObjectAnimal;

    // refactor
    protected FieldObjectAnimalData _animalData;

    private int _directionChangeIntervalSecond;
    private int _impatienceLevel;

    private readonly CompositeDisposable _sparrowRandomPathDisposable = new();
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

        // note
        // 이 값이 낮으면 성격이 급하다 -> 방향 전환을 자주한다.
        _impatienceLevel =
            _randomMaker.Next(DIRECTION_CHANGE_INTERVAL_SECOND_MAX / 2, DIRECTION_CHANGE_INTERVAL_SECOND_MAX);

        ExceptionHelper.CheckNullException(_animalData, "_sparrowData is null");
    }

    protected override void BindEvent()
    {
        base.BindEvent();

        _fieldObjectAnimal.OnCollision.Subscribe(OnCollision).AddTo(_disposable);
        _animalData.OnAnimalStateChanged.Subscribe(OnChangeAnimalState).AddTo(_disposable);

        BindWalkRandomEvent();
    }

    // refactor
    // 네이밍 너무 어려움
    private void BindWalkRandomEvent()
    {
        Observable.Interval(TimeSpan.FromSeconds(DIRECTION_CHANGE_INTERVAL_UPDATE_PERIOD_SECOND))
            .Subscribe(_ =>
            {
                _directionChangeIntervalSecond = _randomMaker.Next(0, DIRECTION_CHANGE_INTERVAL_SECOND_MAX);
            }).AddTo(_disposable);

        Observable.Interval(TimeSpan.FromSeconds(_impatienceLevel))
            .Subscribe(_ => { ChangeDirectionRandomlyIfWalk(); })
            .AddTo(_disposable);
    }

    #endregion

    #region 4-1. EventHandlers - Normal

    // note : SparrowData에서 state를 바꾸면 ReactiveProperty로 인해 콜이 된다.
    public void OnChangeAnimalState(EAnimalState changedState)
    {
        _currentAnimalState = changedState;
        _fieldObjectAnimal.ChangeAnimation((int)_currentAnimalState);

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
                OnCollideWithOtherSparrow();
                break;
            case FieldObjectTree:
                OnCollideWithTree();
                break;
        }
    }

    protected virtual void OnCollideWithRock()
    {
        /*_sparrowData.ChangeSparrowState(EAnimalState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);*/
    }

    protected virtual void OnCollideWithEatableEnvironment()
    {
        /*_fieldObjectAnimal.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(EAnimalState.EAT);
        ChangeToWalkStateAfterDelay(EAT_SECOND, HALF_ROTATION);*/
    }

    protected virtual void OnCollideWithOtherSparrow()
    {
        /*_fieldObjectAnimal.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(EAnimalState.ATTACK);

        ChangeToWalkStateAfterDelay(1f, QUARTER_ROTATION);*/
    }

    // test
    //일단 rock
    protected virtual void OnCollideWithTree()
    {
        /*_sparrowData.ChangeSparrowState(EAnimalState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);*/
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // refactor
    // animation마다 요 동작은 동일한데 이 animation 코드
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
        }).AddTo(_sparrowRandomPathDisposable);
    }

    protected sealed override void DisposeCompositeDisposables()
    {
        base.DisposeCompositeDisposables();

        _sparrowRandomPathDisposable?.Dispose();
    }

    #endregion
}