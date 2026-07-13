using System;
using System.Collections.Generic;

// NOTE
// Factory 역할
// Presenter를 통해 Model도 매핑하고 있다.
public class PresenterManager : ManagerBase<PresenterManager>
{
    #region 1. Fields

    private readonly HashSet<PresenterBase> _livedPresenterHashSet = new();
    private readonly Dictionary<Type, Type> _fieldObjectViewModelTypeMatchDictionary = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor
    
    public sealed override void Initialize()
    {
        SetModelMappingDict();
    }

    // REFACTOR
    // View는 자신의 concrete type을 넣어서 presenter를 생성시킨다.
    // 그리고 presenter는 이 Dictionary를 참고해서 Model을 생성한다. 
    // 근데 이러면 Generic이 나은가?
    private void SetModelMappingDict()
    {
        _fieldObjectViewModelTypeMatchDictionary[typeof(FieldObjectPlayer)] = typeof(FieldObjectPlayerData);
        _fieldObjectViewModelTypeMatchDictionary[typeof(FieldObjectSparrow)] = typeof(FieldObjectSparrowData);
        _fieldObjectViewModelTypeMatchDictionary[typeof(FieldObjectDeer)] = typeof(FieldObjectAnimalData);
        _fieldObjectViewModelTypeMatchDictionary[typeof(FieldObjectLand)] = typeof(FieldObjectLandData);
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // NOTE
    // Factory Pattern
    public void CreatePresenter<TPresenter>(IView view) where TPresenter : PresenterBase, new()
    {
        var presenter = new TPresenter();

        presenter.Initialize(view);
        presenter.SetView();
        presenter.BindEvent();

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
