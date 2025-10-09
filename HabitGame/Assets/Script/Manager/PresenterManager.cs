using System;
using System.Collections.Generic;

// note
// 열려있는 Presenter를 관리한다.
// Factory
public class PresenterManager : ManagerBase<PresenterManager>, IManager
{
    #region 1. Fields

    private readonly HashSet<PresenterBase> _livedPresenterHashSet = new();
    private readonly Dictionary<Type, Type> _fieldObjectViewModelTypeMatchDictionary = new();

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
        InitializeFieldFieldObjectViewModelMatchDictionary();
    }
    
    public void LateInitialize()
    {
        //
    }

    private void InitializeFieldFieldObjectViewModelMatchDictionary()
    {
        _fieldObjectViewModelTypeMatchDictionary[typeof(FieldObjectSparrow)] = typeof(FieldObjectAnimalData);
        _fieldObjectViewModelTypeMatchDictionary[typeof(FieldObjectDeer)] = typeof(FieldObjectAnimalData);
        
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

    public void CreatePresenter<TPresenter>(IView view) where TPresenter : PresenterBase, new()
    {
        var presenter = new TPresenter();
        
        presenter.Initialize(view);

        _livedPresenterHashSet.Add(presenter);
    }

    public void TerminatePresenter(PresenterBase presenter)
    {
        if (_livedPresenterHashSet.Contains(presenter))
        {
            _livedPresenterHashSet.Remove(presenter);
        }
    }

    public Type GetModelTypeUsingMatchDictionary(Type typeKey)
    {
        if (_fieldObjectViewModelTypeMatchDictionary.TryGetValue(typeKey, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException();
    }

    #endregion
}