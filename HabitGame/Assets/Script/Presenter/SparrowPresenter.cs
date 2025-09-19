using UniRx;
using UnityEngine;

public class SparrowPresenter : FieldObjectPresenterBase
{
    #region 1. Fields
    
    private FieldObjectSparrow _fieldObjectSparrow;
    private SparrowData _sparrowData;

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
        ExceptionHelper.CheckNullException(_fieldObjectSparrow, "_fieldObjectSparrow is null");

        if (_model is SparrowData sparrowData)
        {
            _sparrowData = sparrowData;
        }
        ExceptionHelper.CheckNullException(_sparrowData, "_sparrowData is null");
        
        BindEvent();
    }


    protected sealed override void BindEvent()
    {
        base.BindEvent();

        _fieldObjectSparrow.OnCollision.Subscribe(OnCollision).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnCollision(Collision collision)
    {
        
        var gameObject = collision.gameObject;
        

        // todo
        // 타입별로 switch - case
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}