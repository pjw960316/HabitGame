using System;
using UniRx;
using UnityEngine;

// NOTE
// InputManager의 책임은 매우 간단하다.
// InputController를 통해 다양한 Unity Input Context를 다양한 C# 타입으로 전달받는다. -> Action들
// Action을 통해 받아온 값이 변경되면 ReactiveProperty로 외부 MVP에 변경을 전달한다.
public class InputManager : ManagerBase<InputManager>, IHasController<InputController>
{
    #region 1. Fields

    // TODO
    // 얘는 지금 InputController Awake에 의존되긴 함. 그래서 null 위험 있음.
    private InputController _inputController;

    // 상태 관리

    private readonly ReactiveProperty<Vector2> _moveVectorProperty = new();
    private readonly ReactiveProperty<FieldObjectBase> _touchedFieldObjectProperty = new();

    #endregion

    #region 2. Properties

    public IObservable<Vector2> OnMoveVectorChanged => _moveVectorProperty;

    public IObservable<FieldObjectBase> OnTouchedFieldObject => _touchedFieldObjectProperty;

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

    public void RegisterController(InputController inputController)
    {
        if (inputController == null)
        {
            Debug.LogError("inputController is not set.");
            return;
        }
        
        _inputController = inputController;

        // NOTE
        // 바인딩의 실행 순서가 중요하므로 이곳에 적는다.
        _inputController.OnMoveEvent += UpdateMoveVector;
        _inputController.OnTouchEvent += UpdateTouchedTarget;
    }
    
    private void UpdateTouchedTarget(Vector2 curTouchPos)
    {
        var ray = CameraManager.Instance.GetRay(curTouchPos);

        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.TryGetComponent<FieldObjectSparrow>(out var sparrow))
            {
                _touchedFieldObjectProperty.Value = sparrow;
            }
        }
    }

    public void UpdateMoveVector(Vector2 vector)
    {
        _moveVectorProperty.Value = vector;
    }

    #endregion
}
