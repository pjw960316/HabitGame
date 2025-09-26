using System;
using UniRx;
using UnityEngine;

public class FieldObjectSparrow : FieldObjectBase
{
    #region 1. Fields

    private const string SPARROW_ANIMATOR_PARAMETER = "Sparrow";

    [SerializeField] protected Animator _sparrowAnimator;
    [SerializeField] private float _sparrowSpeed;

    private readonly Subject<Collision> _onCollision = new();
    private int _sparrowAnimatorParameter;
    private Rigidbody _sparrowRigidBody;
    private Vector3 _sparrowWalkMovement;
    private Collision _currentCollision;

    #endregion

    #region 2. Properties

    public IObservable<Collision> OnCollision => _onCollision;

    // note : 절대 변경하지 마시오. readonly 문법이 불가능.
    public float DefaultSparrowSpeed { get; private set; }

    #endregion

    #region 3. Constructor

    protected sealed override void Initialize()
    {
        base.Initialize();

        _sparrowRigidBody = _myFieldObjectTransform.GetComponent<Rigidbody>();
        ExceptionHelper.CheckNullException(_sparrowRigidBody, "_sparrowRigidBody");

        _sparrowWalkMovement = _myFieldObjectTransform.forward * (_sparrowSpeed * Time.fixedDeltaTime);
        _sparrowAnimatorParameter = Animator.StringToHash(SPARROW_ANIMATOR_PARAMETER);

        DefaultSparrowSpeed = _sparrowSpeed;
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.SPARROW;
    }

    protected sealed override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    #region 4. EventHandlers

    private void FixedUpdate()
    {
        _sparrowRigidBody.MovePosition(_myFieldObjectTransform.position + _sparrowWalkMovement);
    }

    private void OnCollisionEnter(Collision other)
    {
        _currentCollision = other;
        _onCollision.OnNext(other);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected sealed override void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter<SparrowPresenter>(this);
    }

    public void ChangeAnimation(int enumKey)
    {
        _sparrowAnimator.SetInteger(_sparrowAnimatorParameter, enumKey);
    }

    public void ChangeSparrowPath(int angle)
    {
        _myFieldObjectTransform.Rotate(new Vector3(0, angle, 0));
        UpdateSparrowMovement();
    }

    public void RotateToFaceCollisionObject()
    {
        var path = _currentCollision.transform.position - _myFieldObjectTransform.position;
        var facePath = Quaternion.LookRotation(path);
        _myFieldObjectTransform.rotation = facePath;
    }

    public void ChangeSparrowSpeed(float speed)
    {
        _sparrowSpeed = speed;
        UpdateSparrowMovement();
    }

    public void ChangeSparrowDefaultSpeed()
    {
        _sparrowSpeed = DefaultSparrowSpeed;
        UpdateSparrowMovement();
    }

    private void UpdateSparrowMovement()
    {
        _sparrowWalkMovement = _myFieldObjectTransform.forward * (_sparrowSpeed * Time.fixedDeltaTime);
    }

    public void StopSparrowMoving()
    {
        ChangeSparrowSpeed(0f);
    }

    #endregion
}