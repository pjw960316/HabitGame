using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerMono : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] private PlayerInput _playerInput;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        _playerInput.onActionTriggered += OnHandleInput;
    }

    #endregion

    #region 4. EventHandlers
    
    private void OnHandleInput(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<Vector2>();
    }


    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}