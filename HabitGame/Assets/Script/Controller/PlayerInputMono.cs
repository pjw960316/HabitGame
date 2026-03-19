using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// note : inspector를 통해 데이터 받고, 가공하고, 예외처리 하고, Manager에게 열어준다.
public class PlayerInputController : MonoBehaviour
{
    #region 1. Fields
    
    [SerializeField] private PlayerInput _playerInput;

    // InputActionReference 추가하면 Dictionary에 추가하세요
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _touchInput;
    [SerializeField] private InputActionReference _touchInputPosition;

    private Vector2 _curTouchPosition;
    
    private InputManager _inputManager;
    private CameraManager _cameraManager;
    
    private readonly Dictionary<InputAction, Action<InputAction.CallbackContext>> _actionDict = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _cameraManager = CameraManager.Instance;

        _playerInput.onActionTriggered += OnHandleInput;
        
        InitializeActionDictionary();
    }
    
    private void InitializeActionDictionary()
    {
        _actionDict[_moveInput.action] = OnHandleMove;
        _actionDict[_touchInput.action] = OnHandleTouch;
        _actionDict[_touchInputPosition.action] = OnHandleTouchPosition;
    }

    #endregion

    #region 4. EventHandlers

    public void OnHandleInput(InputAction.CallbackContext context)
    {
        var contextState = context.phase;

        if (contextState == InputActionPhase.Started)
        {
            //
        }
        else if (contextState == InputActionPhase.Performed || contextState == InputActionPhase.Canceled)
        {
            if (_actionDict.TryGetValue(context.action, out var handler))
            {
                handler.Invoke(context);
            }
        }
    }

    public void OnHandleMove(InputAction.CallbackContext context)
    {
        var pathPair = context.ReadValue<Vector2>();
        
        _inputManager.UpdateMoveVector(pathPair);
    }
    
    public void OnHandleTouch(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            var ray = _cameraManager.GetRay(_curTouchPosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponent<FieldObjectSparrow>(out var sparrow))
                {
                    _inputManager.UpdateCurTouchTarget(sparrow);
                }
            }
        }
    }
    
    public void OnHandleTouchPosition(InputAction.CallbackContext context)
    {
        _curTouchPosition = context.ReadValue<Vector2>();
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}