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

// NOTE
// Handler 마다 다른 타입의 결과가 나오는 건 당연하다.
public interface IInputHandler
{
    public void HandleInput(InputAction.CallbackContext context);
}

public class MoveHandler : IInputHandler
{
    private readonly Action<Vector2> _onResult;

    public MoveHandler(Action<Vector2> action)
    {
        _onResult = action;
    }
    
    public void HandleInput(InputAction.CallbackContext context)
    {
        var moveVector = context.ReadValue<Vector2>();

        _onResult.Invoke(moveVector);
    }
}

//note 
//이건 클릭을 했는지 아닌지의 여부를 판단만 한다. position이랑 관련 없다.
//position은 매번 touchPosHandler에서 감지
public class TouchHandler : IInputHandler
{
    private readonly Action _onResult;

    public TouchHandler(Action action)
    {
        _onResult = action;
    }
    
    public void HandleInput(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            return;
        }

        _onResult.Invoke();
    }
}

// REFACTOR
// 매 번 pos를 업데이트하는 건 콜이 많이 들어오긴 한다.
public class TouchPosHandler : IInputHandler
{
    private readonly InputController _inputController;

    public TouchPosHandler(InputController inputController)
    {
        _inputController = inputController;
    }
    
    public void HandleInput(InputAction.CallbackContext context)
    {
        var pos = context.ReadValue<Vector2>();

        _inputController.UpdateCurMousePosition(pos);
    }
}

// Note
// 책임은 단순하다.
// 1. 유니티의 인풋만을 분기해서 데이터로 가공한다.  ->  인터페이스를 이용해서 분기했다.
// 2. 가공된 데이터를 InputManager에게 넘겨준다.
public class InputController : MonoBehaviour, IController
{
    #region 1. Fields

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private SerializedDictionary<EInput, InputActionReference> _inspectorInputDict = new();

    private readonly Dictionary<InputAction, EInput> _inputActionDict = new();
    private readonly Dictionary<EInput, IInputHandler> _handlerDict = new();

    private Vector2 _curTouchPosition;

    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnTouchEvent;
        

    #endregion

    #region 2. Properties

    public Vector2 CurTouchPosition => _curTouchPosition;

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        ConnectManagerAndController();

        InitializeInputActionDictionary();

        _playerInput.onActionTriggered += OnHandleInput;
    }

    private void ConnectManagerAndController()
    {
        //refactor : 이거 자체가 위험한지에 대해 고민해라.
        // 내가 필요한 건 사실 시스템이 아니다.
        // controller가 manager랑 연결하는 게 다야! 
        // 물론 이거 보다 더 좋은 구조가 있겠지만 계속 삽질만 하고 진전이 없다.

        var gameManager = GameManager.Instance;
        var targetManager = gameManager.GetManagerByType<InputManager>();
        targetManager.RegisterController(this);
    }

    private void InitializeInputActionDictionary()
    {
        // mapping
        foreach (var kv in _inspectorInputDict)
        {
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

    // NOTE
    // 분기 지점
    // 분기를 인터페이스를 활용한다.
    public void OnHandleInput(InputAction.CallbackContext context)
    {
        var contextState = context.phase;

        if (contextState == InputActionPhase.Started)
        {
        }
        else if (contextState == InputActionPhase.Performed || contextState == InputActionPhase.Canceled)
        {
            var inputEnum = GetInputEnum(context);
            var handler = _handlerDict[inputEnum];

            handler.HandleInput(context);
        }
    }

    private void OnTouch()
    {
        OnTouchEvent?.Invoke(_curTouchPosition);
    }

    private void OnMove(Vector2 moveVector)
    {
        OnMoveEvent?.Invoke(moveVector);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

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
                return new MoveHandler(OnMove);
            case EInput.TOUCH:
                return new TouchHandler(OnTouch);
            case EInput.TOUCH_POS:
                return new TouchPosHandler(this);
        }

        // todo : warning
        return null;
    }

    public void UpdateCurMousePosition(Vector2 vector)
    {
        _curTouchPosition = vector;
    }

    #endregion
}
