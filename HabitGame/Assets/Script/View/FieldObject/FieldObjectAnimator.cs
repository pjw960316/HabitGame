using UnityEngine;

public class FieldObjectAnimator : MonoBehaviour
{
    #region 1. Fields

    private const string ANIMATOR_PARAMETER = "Animal";

    [SerializeField] private Animator _animator;

    private int _animatorIntegerParameter;
    private bool _isInitialized;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        ExceptionHelper.CheckNullException(_animator, "_animator");

        _animatorIntegerParameter = Animator.StringToHash(ANIMATOR_PARAMETER);
        _isInitialized = true;
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void ChangeAnimation(int enumKey)
    {
        Initialize();

        _animator.SetInteger(_animatorIntegerParameter, enumKey);
    }

    #endregion
}
