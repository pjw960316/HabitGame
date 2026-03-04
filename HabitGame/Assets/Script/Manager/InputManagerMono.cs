using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerMono : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] private PlayerInput _playerInput;
    
    // InputActionReference 추가하면 Dictionary에 추가하세요
    [SerializeField] private InputActionReference _moveInput;
    
    private readonly Dictionary<InputAction, Action<InputAction.CallbackContext>> _actionDict = new ();
    
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
        Debug.Log("hi");
    }
    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}