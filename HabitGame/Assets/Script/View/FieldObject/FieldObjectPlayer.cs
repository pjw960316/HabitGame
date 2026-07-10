using UnityEngine;

// note
// FieldObjectAnimalBase와 분리하는 게 맞다.
public sealed class FieldObjectPlayer : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] private float _playerSpeed;
    
    private Rigidbody _playerRigidBody;
    private Collision _currentCollision;
    private FieldObjectAnimator _fieldObjectAnimator; // note : Animator를 사용하려면 해당 컴포넌트를 추가해야 한다.
    private Vector3 _playerMoveDirection;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        base.Initialize();

        InitializeAnimator();

        _playerRigidBody = FieldObjectTransform.GetComponent<Rigidbody>();
        ExceptionHelper.CheckNullException(_playerRigidBody, "_rigidBody");
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.PLAYER;
    }

    protected override void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter<FieldObjectPlayerPresenter>(this);
    }

    private void InitializeAnimator()
    {
        if (TryGetComponent(out _fieldObjectAnimator))
        {
            _fieldObjectAnimator.Initialize();
        }
    }

    protected override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    #region 4. EventHandlers

    private void FixedUpdate()
    {
        if (_playerMoveDirection == Vector3.zero)
        {
            return;
        }

        var movement = _playerMoveDirection * (_playerSpeed * Time.fixedDeltaTime);
        _playerRigidBody.MovePosition(FieldObjectTransform.position + movement);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void ChangePlayerMoveDirection(Vector3 direction)
    {
        _playerMoveDirection = direction;

        if (_playerMoveDirection == Vector3.zero)
        {
            return;
        }

        FieldObjectTransform.rotation = Quaternion.LookRotation(_playerMoveDirection);
    }

    public void ChangeAnimation(int enumKey)
    {
        if (_fieldObjectAnimator == null)
        {
            Debug.LogWarning($"{name} has no FieldObjectAnimator. Animation change skipped.");
            return;
        }

        _fieldObjectAnimator.ChangeAnimation(enumKey);
    }

    #endregion
}
