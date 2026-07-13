using System;
using UniRx;

public sealed class FieldObjectPlayerData : IModel
{
    #region 1. Fields

    private readonly ReactiveProperty<EPlayerState> _playerState = new();

    #endregion

    #region 2. Properties

    public IObservable<EPlayerState> OnPlayerStateChanged => _playerState;

    #endregion

    #region 3. Constructor

    // NOTE
    // CreateInstance 때문에 public 유지해야 합니다.
    public FieldObjectPlayerData()
    {
        Initialize();
    }

    private void Initialize()
    {
        _playerState.Value = EPlayerState.IDLE;
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    public void ChangePlayerState(EPlayerState changedState)
    {
        _playerState.Value = changedState;
    }

    public EPlayerState GetPlayerState()
    {
        return _playerState.Value;
    }

    public void Terminate()
    {
        _playerState?.Dispose();
    }

    #endregion
}

// NOTE
// Animator의 condition과 다르지 않도록 주의
public enum EPlayerState
{
    IDLE = 0,
    MOVE = 1
}
