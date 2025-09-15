public class FieldObjectSparrow : FieldObjectBase
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();

        // test code start
        FieldObjectAnimator.Play("Walk");
    }

    protected sealed override void CreatePresenterByManager()
    {
        //
    }

    protected sealed override void BindEvent()
    {
        //
    }

    #endregion

    #region 4. EventHandlers

    private void FixedUpdate()
    {
        //
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}