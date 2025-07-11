using UniRx;
using UnityEngine;

public abstract class PresenterBase : IPresenter
{
    #region 1. Fields

    protected IView View;
    protected IModel Model;
    protected readonly CompositeDisposable Disposable = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public virtual void Initialize(IView view)
    {
        Debug.Log("PresenterBase::Initialize");
        View = view;
        
        SetModel(View);
    }

    #endregion

    #region 4. Methods

    private void SetModel(IView view)
    {
        //test
        Model = SoundManager.Instance.SoundData;
    }

    //default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}