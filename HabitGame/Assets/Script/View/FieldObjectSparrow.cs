using System;
using UniRx;
using UnityEngine;

public class FieldObjectSparrow : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] protected Animator _sparrowAnimator;
    [SerializeField] private float _sparrowSpeed;
    private readonly Subject<Collision> _onCollision = new();

    private Rigidbody _sparrowRigidBody;
    private Vector3 _forwardVector;
    private Vector3 _sparrowWalkMovement;

    #endregion

    #region 2. Properties

    public IObservable<Collision> OnCollision => _onCollision;

    #endregion

    #region 3. Constructor

    protected sealed override void Initialize()
    {
        base.Initialize();

        _sparrowRigidBody = _myFieldObjectTransform.GetComponent<Rigidbody>();
        ExceptionHelper.CheckNullException(_sparrowRigidBody, "_sparrowRigidBody");

        _forwardVector = _myFieldObjectTransform.forward;
        _sparrowWalkMovement = _forwardVector * (_sparrowSpeed * Time.fixedDeltaTime);
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.SPARROW;
    }

    protected sealed override void BindEvent()
    {
        //
    }

    #endregion

    #region 4. EventHandlers

    private void FixedUpdate()
    {
        _sparrowRigidBody.MovePosition(_myFieldObjectTransform.position + _sparrowWalkMovement);
    }

    private void OnCollisionEnter(Collision other)
    {
        //RotateSparrow();

        _onCollision.OnNext(other);
    }

    public void ChangeAnimation(int animID)
    {
        _sparrowAnimator.SetBool(animID, true);
    }

    // note
    // 참새를 돌려서 걷는 방향 변경

    //test
    private void RotateSparrow()
    {
        var newRotation = Quaternion.Euler(0f, 180f, 0f) * _sparrowRigidBody.rotation;
        _sparrowRigidBody.MoveRotation(newRotation);

        // 새로운 바라보는 방향을 기준으로 걷기 벡터 재계산
        _forwardVector = _sparrowRigidBody.transform.forward;
        _sparrowSpeed = 0f;
        _sparrowWalkMovement = _forwardVector * (_sparrowSpeed * Time.fixedDeltaTime);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected sealed override void CreatePresenterByManager()
    {
        _presenterManager.CreateFieldObjectPresenter<SparrowPresenter>(this);
    }

    #endregion
}