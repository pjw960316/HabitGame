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

        _sparrowWalkMovement = _myFieldObjectTransform.forward * (_sparrowSpeed * Time.fixedDeltaTime);
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
        _onCollision.OnNext(other);
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

    public void ChangeAnimation(int enumKey)
    {
        _sparrowAnimator.SetInteger("Sparrow", enumKey);
    }

    public void RotateSparrow(int angle)
    {
        _myFieldObjectTransform.Rotate(new Vector3(0, angle, 0));
    }

    public void ChangeSparrowSpeed(float speed)
    {
        _sparrowSpeed = speed;
        _sparrowWalkMovement = _myFieldObjectTransform.forward * (_sparrowSpeed * Time.fixedDeltaTime);
    }
    #endregion
}