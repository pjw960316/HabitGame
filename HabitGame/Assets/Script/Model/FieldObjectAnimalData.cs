using System;
using UniRx;

public class FieldObjectAnimalData : IModel
{
    #region 1. Fields

    private readonly ReactiveProperty<EAnimalState> _animalState = new();

    #endregion

    #region 2. Properties

    public IObservable<EAnimalState> OnAnimalStateChanged => _animalState;

    #endregion

    #region 3. Constructor

    public FieldObjectAnimalData()
    {
        Initialize();
    }

    private void Initialize()
    {
        _animalState.Value = EAnimalState.WALK;
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void ChangeAnimalState(EAnimalState changedState)
    {
        _animalState.Value = changedState;
    }

    public EAnimalState GetAnimalState()
    {
        return _animalState.Value;
    }

    #endregion

    public void Terminate()
    {
        _animalState?.Dispose();
    }
}

// note
// Animator의 condition과 다르지 않도록 주의
public enum EAnimalState
{
    IDLE = 0,
    WALK = 1,
    FLY = 2,
    EAT = 3,
    ATTACK = 4,
    SPIN = 5,
    RUN = 6,
}