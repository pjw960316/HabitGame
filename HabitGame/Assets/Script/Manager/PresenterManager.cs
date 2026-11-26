using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

// NOTE
// Factory 역할
// Presenter를 통해 Model도 매핑하고 있다.

// NOTE
// Presenter의 관리는 여기서 한다.
// UIManager에서 하지 않는다.
public class PresenterManager : ManagerBase<PresenterManager>
{
    #region 1. Fields

    private UIManager _uiManager;

    private readonly HashSet<PresenterBase> _livedPresenterHashSet = new();
    private readonly Dictionary<Type, Type> _fieldObjectViewModelTypeMatchDictionary = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor
    
    public sealed override void Initialize()
    {
        _uiManager = UIManager.Instance;

        SetModelMappingDict();
    }

    // TODO
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

    public sealed override void BindEvent()
    {
        _uiManager.OnOpenPopup.Subscribe(BroadcastPopupOpened);
    }

    #region 4. EventHandlers

    // NOTE
    // 팝업이 열리면 Presenter 전체에게 전달한다.
    // 그러면 Presenter는 다른 View의 이벤트를 걸 수 있다.
    
    // WARNING
    // 자신과 직접 묶인 (MVP) view의 생성 때도 호출된다.
    // 이중 바인딩이 되지 않게 주의해야 한다.
    // 자신과 생명주기가 다른 Popup의 생성 때 UniRx Subscribe 하기 위한 용도
    private void BroadcastPopupOpened(UIPopupBase popup)
    {
        foreach (var presenter in _livedPresenterHashSet)
        {
            presenter.OnPopupOpened(popup);
        }
    }

    #endregion

    #region 5. Methods


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
