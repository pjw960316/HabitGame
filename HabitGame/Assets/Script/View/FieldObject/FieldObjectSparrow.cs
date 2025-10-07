using UnityEngine;

public class FieldObjectSparrow : FieldObjectAnimalBase
{
    #region 1. Fields

    private const string SPARROW_ANIMATOR_PARAMETER = "Sparrow";

    [SerializeField] private Animator _sparrowAnimator;

    private int _sparrowAnimatorParameter;

    #endregion

    #region 2. Properties

//

    #endregion

    #region 3. Constructor

    protected sealed override void Initialize()
    {
        base.Initialize();

        _sparrowAnimatorParameter = Animator.StringToHash(SPARROW_ANIMATOR_PARAMETER);
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

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected sealed override void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter<FieldObjectSparrowPresenter>(this);
    }

    public void ChangeAnimation(int enumKey)
    {
        _sparrowAnimator.SetInteger(_sparrowAnimatorParameter, enumKey);
    }

    #endregion
}