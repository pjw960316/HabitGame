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

    protected EAnimalState _currentSparrowState;
    
    private FieldObjectAnimalBase _fieldObjectAnimal;
    
    // refactor
    protected SparrowData _sparrowData;
    
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
        // refactor : 이거 상위 타입으로, 의외로 model은 공용 Animation이니.
        // 일단 sparrow 붙여서
        if (_model is SparrowData sparrowData)
        {
            _sparrowData = sparrowData;
        }

        // note
        // 이 값이 낮으면 성격이 급하다 -> 방향 전환을 자주한다.
        _impatienceLevel =
            _randomMaker.Next(DIRECTION_CHANGE_INTERVAL_SECOND_MAX / 2, DIRECTION_CHANGE_INTERVAL_SECOND_MAX);
        
        ExceptionHelper.CheckNullException(_sparrowData, "_sparrowData is null");
    }

    protected override void BindEvent()
    {
        base.BindEvent();

        _fieldObjectAnimal.OnCollision.Subscribe(OnCollision).AddTo(_disposable);
        
        // refactor 
        // 이거도 여기 있어야 함 -> 상태 변경 시 모두 Animation 바뀔 필요 있음.
        //_sparrowData.OnSparrowStateChanged.Subscribe(OnChangeSparrowState).AddTo(_disposable);
        
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
    /*public void OnChangeSparrowState(ESparrowState changedState)
    {
        _currentSparrowState = changedState;
        
        //refactor
        //이건 하위
        //_fieldObjectAnimal.ChangeAnimation((int)_currentSparrowState);

        // log
        // Debug.Log($"{_fieldObjectSparrow.name}'s State Change -> {_currentSparrowState}");

        if (_currentSparrowState == ESparrowState.WALK)
        {
            _fieldObjectAnimal.ChangeAnimalDefaultSpeed();
        }
    }*/

    /*private void OnChangeSparrowSpinState(Unit _)
    {
        _fieldObjectAnimal.ChangeAnimalSpeedZero();
        _sparrowData.ChangeSparrowState(ESparrowState.SPIN);
        
        ChangeToWalkStateAfterDelay(DANCE_SECOND, QUARTER_ROTATION);
    }*/

    #endregion

    # region 4-2. EventHandlers - Collision

    // note : 모든 동물 마다의 OnCollision은 다르지만 구현을 해야지
    protected abstract void OnCollision(Collision collision);
    // {
    //     var fieldObjectBase = collision.gameObject.GetComponentInParent<FieldObjectBase>();
    //     ExceptionHelper.CheckNullException(fieldObjectBase, "fieldObjectBase script X");
    //     
    //     _fieldObjectAnimal.StopAnimalMoving();
    //
    //     switch (fieldObjectBase)
    //     {
    //         case FieldObjectRock:
    //             OnCollideWithRock();
    //             break;
    //         case FieldObjectMushroom:
    //         case FieldObjectFlower:
    //             OnCollideWithEatableEnvironment();
    //             break;
    //         case FieldObjectSparrow:
    //             OnCollideWithOtherSparrow();
    //             break;
    //         case FieldObjectTree:
    //             OnCollideWithTree();
    //             break;
    //     }
    // }

    /*private void OnCollideWithRock()
    {
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    private void OnCollideWithEatableEnvironment()
    {
        _fieldObjectAnimal.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(ESparrowState.EAT);
        ChangeToWalkStateAfterDelay(EAT_SECOND, HALF_ROTATION);
    }

    private void OnCollideWithOtherSparrow()
    {
        _fieldObjectAnimal.RotateToFaceCollisionObject();
        _sparrowData.ChangeSparrowState(ESparrowState.ATTACK);

        ChangeToWalkStateAfterDelay(1f, QUARTER_ROTATION);
    }

    // test
    //일단 rock
    private void OnCollideWithTree()
    {
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }*/

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // refactor
    // animation마다 요 동작은 동일한데 이 animation 코드
    private void ChangeToWalkStateAfterDelay(float delaySeconds, int sparrowRotationDegree)
    {
        Observable.Timer(TimeSpan.FromSeconds(delaySeconds)).Subscribe(_ =>
        {
            _fieldObjectAnimal.ChangeAnimalPath(sparrowRotationDegree);
            
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
