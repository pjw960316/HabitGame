public class SparrowPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private FieldObjectSparrow _fieldObjectSparrow;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        if (_view is FieldObjectSparrow sparrow)
        {
            _fieldObjectSparrow = sparrow;
        }
        
        BindEvent();
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

    // 

    #endregion
}