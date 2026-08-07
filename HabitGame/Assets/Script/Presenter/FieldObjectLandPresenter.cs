public sealed class FieldObjectLandPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private FieldObjectLand _fieldObjectLand;
    private FieldObjectLandData _fieldObjectLandData;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void InitializeView()
    {
        base.InitializeView();

        _fieldObjectLand = _view as FieldObjectLand;
        ExceptionHelper.CheckNullException(_fieldObjectLand, "_fieldObjectLand");
    }

    protected override void InitializeModel()
    {
        base.InitializeModel();

        _fieldObjectLandData = _model as FieldObjectLandData;
        ExceptionHelper.CheckNullException(_fieldObjectLandData, "_fieldObjectLandData");
    }

    public override void SetView()
    {
        // NOTE
        // 나중에 필요하면.
    }

    public override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    //


    //

    #endregion
}
