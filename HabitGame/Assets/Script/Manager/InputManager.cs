using System;
using UniRx;
using UnityEngine;

public class InputManager : ManagerBase<InputManager>
{
    #region 1. Fields

    // REFACTOR
    // 얘는 지금 InputController Awake에 의존되긴 함. 그래서 null 위험 있음.
    private InputController _inputController;
    
    // 상태 관리
    private Vector2 _moveVector;

    private readonly ReactiveProperty<FieldObjectBase> _touchedFieldObject = new();
    #endregion

    #region 2. Properties

    public Vector2 MoveVector => _moveVector;
    
    public IObservable<FieldObjectBase> OnTouchedFieldObject => _touchedFieldObject;

    #endregion

    #region 3. Constructor

    public void RegisterController(InputController controller)
    {
        _inputController = controller;

        BindInputControllerEvent();
    }

    private void BindInputControllerEvent()
    {
        _inputController.OnTouchEvent += OnTouchScreen;
    }

    #endregion

    #region 4. EventHandlers

    private void OnTouchScreen(Vector2 curTouchPos)
    {
        var ray = CameraManager.Instance.GetRay(curTouchPos);

        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.TryGetComponent<FieldObjectSparrow>(out var sparrow))
            {
                _touchedFieldObject.Value = sparrow;
            }
        }
    }
    /*private void OnHandleInput(IInputResult result)
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
    }*/

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods
    
    

    
    public void UpdateMoveVector(Vector2 vector)
    {
        _moveVector = vector;
    }


    #endregion
}