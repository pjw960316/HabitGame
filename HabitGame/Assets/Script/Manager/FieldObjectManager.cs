using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Random = System.Random;

// Note
// fieldObjectManager가 fieldObject보다 먼저 생성된다.
// Manager들은 Unity 세계에서 View를 알기 때문에 View를 통해 Presenter를 얻어와야 한다.
public class FieldObjectManager : ManagerBase<FieldObjectManager>
{
    #region 1. Fields

    // Note
    // (Key , Value)  =>  (View의 InstanceID , FieldObjectPresenterBase)
    private readonly Dictionary<int, FieldObjectPresenterBase> _fieldObjectPresenterDict = new(); 
    
    private readonly Random _randomMaker = new();
    private InputManager _inputManager;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    // TODOㅑ
    // Manager는 어차피 계속 존재하니까 disposable이 필요한가?  
    public sealed override void Initialize()
    {
        _inputManager = InputManager.Instance;

        _inputManager.OnTouchedFieldObject
            .Subscribe(OnSelectedFieldObject);
        //.AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnSelectedFieldObject(FieldObjectBase targetFieldObject)
    {
        if (targetFieldObject == null)
        {
            return;
        }

        var targetInstanceID = targetFieldObject.InstanceID;

        foreach (var kv in _fieldObjectPresenterDict)
        {
            if (kv.Value is FieldObjectSparrowPresenter sparrowPresenter)
            {
                sparrowPresenter.UpdateTarget(kv.Key == targetInstanceID);
            }
        }
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // note
    // FieldObject가 생성되면 호출해서 Dictionary에 추가한다.
    // 의존성?
    public void RegisterFieldObjectPresenter(FieldObjectPresenterBase fieldObjectPresenterBase)
    {
        var instanceID = fieldObjectPresenterBase.GetFieldObjectInstanceID();

        if (!_fieldObjectPresenterDict.TryAdd(instanceID, fieldObjectPresenterBase))
        {
            throw new InvalidOperationException("키가 고유인데 중복됩니다.");
        }
    }


    public FieldObjectSparrow GetRandomSparrow()
    {
        var sparrowCount = 0;

        foreach (var kv in _fieldObjectPresenterDict)
        {
            var fieldObjectPresenter = kv.Value;

            if (fieldObjectPresenter is FieldObjectSparrowPresenter)
            {
                sparrowCount++;
            }
        }

        if (sparrowCount == 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        var randValue = _randomMaker.Next(0, sparrowCount);
        var tmpValue = 0;

        var sparrowPresenters = _fieldObjectPresenterDict
            .Select(kv => kv.Value)
            .OfType<FieldObjectSparrowPresenter>();

        foreach (var sparrowPresenter in sparrowPresenters)
        {
            if (tmpValue == randValue)
            {
                return sparrowPresenter.GetFieldObjectSparrow();
            }

            tmpValue++;
        }

        return null;
    }
    public TFieldObjectPresenter GetFieldObjectPresenter<TFieldObjectPresenter>(int instanceID) where TFieldObjectPresenter : FieldObjectPresenterBase
    {
        if (!_fieldObjectPresenterDict.TryGetValue(instanceID, out var presenter))
        {
            throw new KeyNotFoundException(
                $"FieldObjectPresenter not found. InstanceID: {instanceID}");
        }

        if (presenter is not TFieldObjectPresenter typedPresenter)
        {
            throw new InvalidCastException(
                $"Invalid presenter type. InstanceID: {instanceID}, " +
                $"Expected: {typeof(TFieldObjectPresenter).Name}, Actual: {presenter.GetType().Name}");
        }

        return typedPresenter;
    }

    #endregion
}