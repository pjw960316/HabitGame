// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// //using UnityEngine.InputSystem;
//
// // note : 라우팅
// public class InputManager : ManagerBase<InputManager>
// {
//     #region 1. Fields
//
//     private readonly Dictionary<InputAction, Action<InputAction.CallbackContext>> _actionDict = new();
//     private Vector2 _curPointerPos = new();
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
//         InitializeActionDictionary();
//
//         if (_playerInput.currentActionMap.Count() != _actionDict.Count)
//         {
//             Debug.LogError("PlayerInput Component의 액션 개수와 _actionDict의 액션 개수가 다르다.");
//         }
//
//         _playerInput.onActionTriggered += OnHandleInput;
//     }
//
//     
//
//     #endregion
//
//     #region 4. EventHandlers
//
//     public void OnHandleInput(InputAction.CallbackContext context)
//     {
//         var contextState = context.phase;
//
//         if (contextState == InputActionPhase.Started)
//         {
//         }
//         else if (contextState == InputActionPhase.Performed)
//         {
//             if (_actionDict.TryGetValue(context.action, out var handler))
//             {
//                 handler.Invoke(context);
//             }
//         }
//         else if (contextState == InputActionPhase.Canceled)
//         {
//         }
//     }
//
//     public void OnHandleMove(InputAction.CallbackContext context)
//     {
//         var pathPair = context.ReadValue<Vector2>();
//         Debug.Log($"{pathPair}");
//     }
//     
//     public void OnHandleTouch(InputAction.CallbackContext context)
//     {
//         var ray = CameraManager.Instance.RequestRay(_curPointerPos);
//
//         if (Physics.Raycast(ray, out RaycastHit hit))
//         {
//             Debug.Log($"{_curPointerPos}");
//             Debug.Log($"Hit : {hit.collider.name}");
//         }
//     }
//     
//     // note
//     // 큰 struct 보다는 작은 vector2를 전달하고 싶다.
//     // 그러나 dict value의 타입에 맞지 않음. 
//     public void OnHandleTouchPosition(InputAction.CallbackContext context)
//     {
//         _curPointerPos = context.ReadValue<Vector2>();
//     }
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