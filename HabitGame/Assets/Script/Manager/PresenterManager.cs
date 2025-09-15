using System.Collections.Generic;

// note
// 열려있는 Presenter를 관리한다.
// Factory
public class PresenterManager : ManagerBase<PresenterManager>, IManager
{
    #region 1. Fields

    private readonly HashSet<UIPresenterBase> _livedPresenterHashSet = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
    }

    public void Initialize()
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

    public void SetModel(IEnumerable<IModel> models)
    {
        //
    }

    public void CreatePresenter<TPresenter>(IView view) where TPresenter : UIPresenterBase, new()
    {
        var presenter = new TPresenter();
        presenter.Initialize(view);

        _livedPresenterHashSet.Add(presenter);
    }

    public void TerminatePresenter(UIPresenterBase presenter)
    {
        if (_livedPresenterHashSet.Contains(presenter))
        {
            _livedPresenterHashSet.Remove(presenter);
        }
    }

    #endregion
}