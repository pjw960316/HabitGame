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
        
        
        _currentAnimalState = _animalData.GetAnimalState();

        

        BindEvent();
    }


    protected sealed override void BindEvent()
    {
        base.BindEvent();
        
        
        _myCharacterManager.OnUpdateRoutineSuccess.Subscribe(OnChangeSparrowSpinState).AddTo(_disposable);
    }


    #endregion

    #region 4-1. EventHandlers - Normal

    

    private void OnChangeSparrowSpinState(Unit _)
    {
        _fieldObjectSparrow.ChangeAnimalSpeedZero();
        _animalData.ChangeAnimalState(EAnimalState.SPIN);
        
        ChangeToWalkStateAfterDelay(DANCE_SECOND, QUARTER_ROTATION);
    }

    #endregion

    # region 4-2. EventHandlers - Collision

    // refactor
    // 이거 상위로 올리고
    // 하위에서 On들 override
    /*protected sealed override void OnCollision(Collision collision)
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
    }*/

    protected override void OnCollideWithRock()
    {
        _animalData.ChangeAnimalState(EAnimalState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    protected override void OnCollideWithEatableEnvironment()
    {
        _fieldObjectSparrow.RotateToFaceCollisionObject();
        _animalData.ChangeAnimalState(EAnimalState.EAT);
        ChangeToWalkStateAfterDelay(EAT_SECOND, HALF_ROTATION);
    }

    protected override void OnCollideWithOtherSparrow()
    {
        _fieldObjectSparrow.RotateToFaceCollisionObject();
        _animalData.ChangeAnimalState(EAnimalState.ATTACK);

        ChangeToWalkStateAfterDelay(1f, QUARTER_ROTATION);
    }

    // test
    //일단 rock
    protected override void OnCollideWithTree()
    {
        _animalData.ChangeAnimalState(EAnimalState.FLY);

        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods
    

    /*protected sealed override void DisposeCompositeDisposables()
    {
        base.DisposeCompositeDisposables();

        _sparrowRandomPathDisposable?.Dispose();
    }*/

    #endregion
}