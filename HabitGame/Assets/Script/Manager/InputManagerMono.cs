using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerMono : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] private PlayerInput _playerInput;

    // InputActionReference 추가하면 Dictionary에 추가하세요
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _touchInput;
    [SerializeField] private InputActionReference _touchInputPosition;

    private readonly Dictionary<InputAction, Action<InputAction.CallbackContext>> _actionDict = new();

    private Vector2 _curPointerPos = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        InitializeActionDictionary();

        if (_playerInput.currentActionMap.Count() != _actionDict.Count)
        {
            Debug.LogError("PlayerInput Component의 액션 개수와 _actionDict의 액션 개수가 다르다.");
        }

        _playerInput.onActionTriggered += OnHandleInput;
    }

    private void InitializeActionDictionary()
    {
        _actionDict[_moveInput.action] = OnHandleMove;
        _actionDict[_touchInput.action] = OnHandleTouch;
        _actionDict[_touchInputPosition.action] = OnHandleTouchPosition;
    }

    #endregion

    #region 4. EventHandlers

    private void OnHandleInput(InputAction.CallbackContext context)
    {
        var contextState = context.phase;

        if (contextState == InputActionPhase.Started)
        {
        }
        else if (contextState == InputActionPhase.Performed)
        {
            if (_actionDict.TryGetValue(context.action, out var handler))
            {
                handler.Invoke(context);
            }
        }
        else if (contextState == InputActionPhase.Canceled)
        {
        }
    }

    private void OnHandleMove(InputAction.CallbackContext context)
    {
        var pathPair = context.ReadValue<Vector2>();
        Debug.Log($"{pathPair}");
    }
    
    private void OnHandleTouch(InputAction.CallbackContext context)
    {
        Debug.Log($"{_curPointerPos}");

        var ray = CameraManager.Instance.RequestRay(_curPointerPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"Hit : {hit.collider.name}");
        }

        Debug.Log("OnHandleTouch");
    }
    
    // note
    // 큰 struct 보다는 작은 vector2를 전달하고 싶다.
    // 그러나 dict value의 타입에 맞지 않음. 
    private void OnHandleTouchPosition(InputAction.CallbackContext context)
    {
        _curPointerPos = context.ReadValue<Vector2>();
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}