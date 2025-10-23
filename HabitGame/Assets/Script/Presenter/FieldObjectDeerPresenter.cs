public class FieldObjectDeerPresenter : FieldObjectAnimalPresenterBase
{
    #region 1. Fields

    private FieldObjectDeer _fieldObjectDeer;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);
    }

    protected sealed override void InitializeView()
    {
        base.InitializeView();

        _fieldObjectDeer = _view as FieldObjectDeer;
        ExceptionHelper.CheckNullException(_fieldObjectDeer, "_fieldObjectDeer is null");
    }

    public sealed override void SetView()
    {
        // note : 나중에 필요하면.
    }

    public sealed override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    # region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    //

    #endregion
}