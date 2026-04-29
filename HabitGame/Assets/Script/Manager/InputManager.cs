using System;
using UniRx;
using UnityEngine;

public class InputManager : ManagerBase<InputManager>
{
    #region 1. Fields

    // refactor : 얘는 지금 InputController Awake에 의존되긴 함. 그래서 null 위험 있음.
    private InputController _inputController;
    
    // 상태 관리
    private Vector2 _moveVector;
    private Vector2 _curTouchPos;

    private readonly ReactiveProperty<FieldObjectBase> _touchedFieldObject = new();
    #endregion

    #region 2. Properties

    public Vector2 MoveVector => _moveVector;
    public Vector2 CurTouchPos => _curTouchPos;

    public IObservable<FieldObjectBase> OnTouchedFieldObject => _touchedFieldObject;

    #endregion

    #region 3. Constructor

    //

    #endregion

    #region 4. EventHandlers

    private void OnHandleInput(IInputResult result)
    {
        switch (result)
        {
            case MoveResult move:
                UpdateMoveVector(move.Direction);
                break;

            case TouchResult touch:
                UpdateCurTouchTarget(touch.Target);
                break;

            case TouchPosResult pos:
                UpdateCurTouchPosition(pos.Position);
                break;
        }
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods
    
    public void RegisterController(InputController controller)
    {
        _inputController = controller;
    }
    
    public void UpdateMoveVector(Vector2 vector)
    {
        _moveVector = vector;
    }

    public void UpdateCurTouchTarget(FieldObjectBase fieldObject)
    {
        if (fieldObject as FieldObjectSparrow)
        {
            _touchedFieldObject.Value = fieldObject;
        }
    }

    public void UpdateCurTouchPosition(Vector2 vector)
    {
        _curTouchPos = vector;
    }

    #endregion
}