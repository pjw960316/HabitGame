using UniRx;
using UnityEngine;

public sealed class FieldObjectPlayerPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private FieldObjectPlayer _fieldObjectPlayer;
    private FieldObjectPlayerData _fieldObjectPlayerData;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public override void SetView()
    {
        // note : 나중에 필요하면.
    }

    protected override void InitializeView()
    {
        base.InitializeView();

        _fieldObjectPlayer = _view as FieldObjectPlayer;
        ExceptionHelper.CheckNullException(_fieldObjectPlayer, "_fieldObjectPlayer");
    }

    protected override void InitializeModel()
    {
        base.InitializeModel();

        _fieldObjectPlayerData = _model as FieldObjectPlayerData;
        ExceptionHelper.CheckNullException(_fieldObjectPlayerData, "_fieldObjectPlayerData");
    }

    public override void BindEvent()
    {
        base.BindEvent();

        _inputManager.OnMoveVectorChanged
            .Subscribe(OnMoveInput)
            .AddTo(_disposable);

        _fieldObjectPlayerData.OnPlayerStateChanged
            .Subscribe(OnChangePlayerState)
            .AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnMoveInput(Vector2 moveVector)
    {
        var moveDirection = new Vector3(moveVector.x, 0f, moveVector.y);

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        _fieldObjectPlayer.ChangePlayerMoveDirection(moveDirection);
        _fieldObjectPlayerData.ChangePlayerState(moveDirection == Vector3.zero ? EPlayerState.IDLE : EPlayerState.MOVE);
    }

    private void OnChangePlayerState(EPlayerState changedState)
    {
        _fieldObjectPlayer.ChangeAnimation((int)changedState);
    }

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    //

    #endregion
}
