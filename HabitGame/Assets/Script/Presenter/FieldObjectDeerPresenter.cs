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

        if (_view is FieldObjectDeer deer)
        {
            _fieldObjectDeer = deer;
        }

        ExceptionHelper.CheckNullException(_fieldObjectDeer, "_fieldObjectDeer is null");

        BindEvent();
    }

    protected sealed override void BindEvent()
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