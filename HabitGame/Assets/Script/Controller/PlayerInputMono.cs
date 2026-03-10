// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// // note : inspector를 통해 데이터 받고, 가공하고, 예외처리 하고, Manager에게 열어준다.
// public class PlayerInputController : MonoBehaviour
// {
//     #region 1. Fields
//
//     private InputManager _inputManager;
//     
//     [SerializeField] private PlayerInput _playerInput;
//
//     // InputActionReference 추가하면 Dictionary에 추가하세요
//     [SerializeField] private InputActionReference _moveInput;
//     [SerializeField] private InputActionReference _touchInput;
//     [SerializeField] private InputActionReference _touchInputPosition;
//     
//     private readonly Dictionary<InputAction, Action<>> _actionDict = new(); 
//
//     #endregion
//
//     #region 2. Properties
//
//     //
//
//     #endregion
//
//     #region 3. Constructor
//
//     private void Awake()
//     {
//         _inputManager = InputManager.Instance;
//
//         InitializeActionDictionary();
//     }
//     
//     private void InitializeActionDictionary()
//     {
//         _actionDict[_moveInput.action] = _inputManager.OnHandleMove;
//         _actionDict[_touchInput.action] = _inputManager.OnHandleTouch;
//         _actionDict[_touchInputPosition.action] = _inputManager.OnHandleTouchPosition;
//     }
//
//     #endregion
//
//     #region 4. EventHandlers
//
//     //
//
//     #endregion
//
//     #region 5. Request Methods
//
//     // 
//
//     #endregion
//
//     #region 6. Methods
//
//     // 
//
//     #endregion
// }