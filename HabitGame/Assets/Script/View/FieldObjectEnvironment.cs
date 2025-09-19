// refactor 
// abstract?
public class FieldObjectEnvironment : FieldObjectBase
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
    }

    protected override void InitializeEnumFieldObjectKey()
    {
        // refactor 
        // 얘는 하위에서?
        _eFieldObjectKey = EFieldObject.BUSH;
    }

    protected sealed override void BindEvent()
    {
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

    protected override void CreatePresenterByManager()
    {
        //
    }
}