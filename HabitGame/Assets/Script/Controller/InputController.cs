using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EInput
{
    MOVE,
    TOUCH,
    TOUCH_POS
}

public interface IInputHandler
{
    public void HandleInput(InputAction.CallbackContext context, InputManager inputManager);
}

public class MoveHandler : IInputHandler
{
    public void HandleInput(InputAction.CallbackContext context, InputManager inputManager)
    {
        var pathPair = context.ReadValue<Vector2>();

        InputManager.Instance.UpdateMoveVector(pathPair);
    }
}

public class TouchHandler : IInputHandler
{
    public void HandleInput(InputAction.CallbackContext context, InputManager inputManager)
    {
        if (context.canceled)
        {
            var ray = CameraManager.Instance.GetRay(inputManager.GetCurTouchPos());

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponent<FieldObjectSparrow>(out var sparrow))
                {
                    InputManager.Instance.UpdateCurTouchTarget(sparrow);
                }
            }
        }
    }
}

public class TouchPosHandler : IInputHandler
{
    public void HandleInput(InputAction.CallbackContext context, InputManager inputManager)
    {
        var curTouchPosition = context.ReadValue<Vector2>();
        inputManager.UpdateCurTouchPosition(curTouchPosition);
    }
}

public class InputController : MonoBehaviour, IController
{
    #region 1. Fields

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private SerializedDictionary<EInput, InputActionReference> _inspectorInputDict = new();
    
    private readonly Dictionary<InputAction, EInput> _inputActionDict = new();
    private readonly Dictionary<EInput, IInputHandler> _handlerDict = new();

    private Vector2 _curTouchPosition;

    private InputManager _inputManager;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _inputManager.RegisterController(this);
        
        InitializeInputActionDictionary();
        
        _playerInput.onActionTriggered += OnHandleInput;
    }

    private void InitializeInputActionDictionary()
    {
        // mapping
        foreach (var kv in _inspectorInputDict)
        {
            Debug.Log($"{kv.Value.action} , {kv.Key}");
            _inputActionDict[kv.Value.action] = kv.Key;
        }

        // strategy pattern mapping
        foreach (var kv in _inputActionDict)
        {
            var enumKey = kv.Value;
            _handlerDict[enumKey] = GetHandler(enumKey);
        }
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
            var inputEnum = GetInputEnum(context);
            var handler = _handlerDict[inputEnum];

            handler.HandleInput(context, _inputManager);
        }
    }

    private EInput GetInputEnum(InputAction.CallbackContext context)
    {
        var action = context.action;

        return _inputActionDict[action];
    }

    private IInputHandler GetHandler(EInput eInput)
    {
        switch (eInput)
        {
            case EInput.MOVE:
                return new MoveHandler();
            case EInput.TOUCH:
                return new TouchHandler();
            case EInput.TOUCH_POS:
                return new TouchPosHandler();
        }

        // todo : warning
        return null;
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    //

    #endregion
}