using UnityEngine;

// note
// FieldObjectAnimalBase와 분리하는 게 맞다.
public sealed class FieldObjectPlayer : FieldObjectBase
{
    #region 1. Fields

    private Rigidbody _animalRigidBody;
    private Collision _currentCollision;
    private FieldObjectAnimator _fieldObjectAnimator; // note : Animator를 사용하려면 해당 컴포넌트를 추가해야 한다.

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        base.Initialize();

        InitializeAnimator();

        _animalRigidBody = FieldObjectTransform.GetComponent<Rigidbody>();
        ExceptionHelper.CheckNullException(_animalRigidBody, "_rigidBody");
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        //throw new NotImplementedException();
    }

    protected override void CreatePresenterByManager()
    {
        //throw new NotImplementedException();
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

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    //

    #endregion
}