public class FieldObjectSparrow : FieldObjectAnimalBase
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected sealed override void Initialize()
    {
        base.Initialize();
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

    #region 5. Methods

    // 


    protected sealed override void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter<FieldObjectSparrowPresenter>(this);
    }

    #endregion
}