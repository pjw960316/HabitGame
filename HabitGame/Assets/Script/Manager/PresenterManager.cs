using System;
using System.Collections.Generic;
using System.Linq;

// Note
// MVP 객체들은 Manager와 소통하기 위해 Presenter가 이용된다.
// 그러나 Unity 세계에 존재하는 것은 View다. 하지만 View는 Presenter를 모른다.
// 매칭할 책임이 필요하다.
public class PresenterManager : ManagerBase<PresenterManager>
{
    #region 1. Fields

    private readonly Dictionary<Type, Type> _fieldObjectViewModelTypeMatchDictionary = new();

    // TODO
    // 아니다 -> 얘는 전체 presenter를 관리해야 하므로 이렇게 할 필요 없다.
    private readonly Dictionary<int, FieldObjectPresenterBase> _livedFieldObjectDict = new(); // key = FieldObject (view) 의 InstanceID

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    // refactor 필요한 지 체크
    public sealed override void Initialize()
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

    // NOTE
    // Factory Pattern
    public void CreatePresenter<TPresenter>(IView view) where TPresenter : PresenterBase, new()
    {
        var presenter = new TPresenter();

        presenter.Initialize(view);
        presenter.SetView();
        presenter.BindEvent();

        UpdateFieldObjectDictionary(view, presenter);
    }

    // TODO : 이거 
    private void UpdateFieldObjectDictionary(IView view, PresenterBase presenter)
    {
        if (view is FieldObjectBase fieldObjectBase && presenter is FieldObjectPresenterBase fieldObjectPresenterBase)
        {
            var key = fieldObjectBase.InstanceID;
            _livedFieldObjectDict[key] = fieldObjectPresenterBase;
        }
        
        // TODO
        // UI도 매칭 Dictionary에 업데이트 -> 일단 지금은 필요 없어서
    }

    public void TerminatePresenter(PresenterBase presenter)
    {
        var key = _livedFieldObjectDict
            .FirstOrDefault(pair => pair.Value == presenter).Key;
        _livedFieldObjectDict.Remove(key);
        
        // TODO
        // UI도 
    }

    public TPresenter GetFieldObjectPresenter<TPresenter>(int instanceID)
        where TPresenter : FieldObjectPresenterBase
    {
        if (!_livedFieldObjectDict.TryGetValue(instanceID, out var presenter))
        {
            throw new KeyNotFoundException(
                $"FieldObjectPresenter not found. InstanceID: {instanceID}");
        }

        if (presenter is not TPresenter typedPresenter)
        {
            throw new InvalidCastException(
                $"Invalid presenter type. InstanceID: {instanceID}, " +
                $"Expected: {typeof(TPresenter).Name}, Actual: {presenter.GetType().Name}");
        }

        return typedPresenter;
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