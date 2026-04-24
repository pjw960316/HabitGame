using System;
using UniRx;
using UnityEngine;

public class InputManager : ControllerManagerBase<InputManager,InputController>
{
    #region 1. Fields

    private Vector2 _moveVector;
    private readonly ReactiveProperty<FieldObjectBase> _touchedFieldObject = new();
    private InputController _inputController;
    private Vector2 _curTouchPos;

    #endregion

    #region 2. Properties

    public Vector2 MoveVector => _moveVector;

    public IObservable<FieldObjectBase> OnTouchedFieldObject => _touchedFieldObject;

    #endregion

    #region 3. Constructor

    //

    #endregion

    #region 4. EventHandlers

//

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods
    
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

    public Vector2 GetCurTouchPos()
    {
        return _curTouchPos;
    }

    #endregion
}