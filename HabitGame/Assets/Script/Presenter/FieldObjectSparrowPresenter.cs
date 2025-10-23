using System;
using UniRx;

public class FieldObjectSparrowPresenter : FieldObjectAnimalPresenterBase
{
    #region 1. Fields

    private const int EAT_SECOND = 15;
    private const int SPIN_SECOND = 3;

    private FieldObjectSparrow _fieldObjectSparrow;
    private int _directionChangeIntervalSecond;
    private int _impatienceLevel;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);
    }

    public override void SetView()
    {
        // note : 나중에 필요하면.
    }

    protected sealed override void InitializeView()
    {
        base.InitializeView();

        _fieldObjectSparrow = _view as FieldObjectSparrow;
        ExceptionHelper.CheckNullException(_fieldObjectSparrow, "_fieldObjectSparrow is null");
    }

    public sealed override void BindEvent()
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
        _animalData.BlockChangeState();

        Observable.Timer(TimeSpan.FromSeconds(SPIN_SECOND)).Subscribe(_ =>
        {
            _animalData.AllowChangeState();

            _fieldObjectSparrow.ChangeAnimalPath(QUARTER_ROTATION);
            _animalData.ChangeAnimalState(EAnimalState.WALK);
        }).AddTo(_disposable);
    }

    #endregion

    # region 4-2. EventHandlers - Collision

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

    protected override void OnCollideWithTree()
    {
        _animalData.ChangeAnimalState(EAnimalState.IDLE);
        ChangeToWalkStateAfterDelay(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND, HALF_ROTATION);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    //

    #endregion
}