using System;
using UniRx;
using UnityEngine;

public class SparrowPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private const float COLLIDED_ROCK_ANIMATION_CHANGE_SECOND = 1f;
    private const int HALF_TURN_ANGLE = 180;

    private FieldObjectSparrow _fieldObjectSparrow;
    private SparrowData _sparrowData;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

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

        BindEvent();
    }


    protected sealed override void BindEvent()
    {
        base.BindEvent();

        _fieldObjectSparrow.OnCollision.Subscribe(OnCollision).AddTo(_disposable);
        _sparrowData.OnSparrowStateChanged.Subscribe(OnChangeSparrowState).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    // note : 충돌 개체 별 분기
    private void OnCollision(Collision collision)
    {
        var fieldObjectEnvironmentBase =
            collision.gameObject.GetComponent<FieldObjectBase>() as FieldObjectEnvironmentBase;

        switch (fieldObjectEnvironmentBase)
        {
            case FieldObjectRock:
                OnCollideWithRock();
                break;
            case FieldObjectMushroom:
            case FieldObjectFlower:
                OnCollideWithEatableEnvironment();
                break;
        }
    }

    private void OnCollideWithRock()
    {
        // log
        Debug.Log("Collide with Rock");

        // note : 박아서 스턴 된 애니메이션 의도
        _fieldObjectSparrow.ChangeSparrowSpeed(0f);
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);

        // todo
        // 의도 되지 않은 타이밍에 콜 되는 거 막기
        Observable.Timer(TimeSpan.FromSeconds(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND)).Subscribe(_ =>
        {
            _fieldObjectSparrow.RotateSparrow(HALF_TURN_ANGLE);
            _sparrowData.ChangeSparrowState(ESparrowState.WALK);
            _fieldObjectSparrow.ChangeSparrowSpeed(3f);
        }).AddTo(_disposable);
    }

    private void OnCollideWithEatableEnvironment()
    {
        // log
        Debug.Log("Collide with Flower 또는 Mushroom");

        _sparrowData.ChangeSparrowState(ESparrowState.EAT);
    }

    // note : model's ReactiveProperty Event
    public void OnChangeSparrowState(ESparrowState changedState)
    {
        _fieldObjectSparrow.ChangeAnimation((int)changedState);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}