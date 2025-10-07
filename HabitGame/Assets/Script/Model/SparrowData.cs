// note
// FieldObject의 Model Script를 구현하면서 느낀 점
// 이전의 UI의 Model 쪽은 Manager와 경계가 모호하다.
// 우선 얘라도 확실하게 Model = Data / Manager = Model들을 관리 -> 이 단일 책임을 들고 가자.

// todo
// 현재 얘는 GameStartMonoManager의 SetModel에 포함되지 않음.
// 그들은 ScriptableObject + XML만 로드 했음.
// 얘는 그냥 데이터 스크립트기 때문.
// 그리고 -> 생명 주기도 있어야 한다.
// 생성 자체를 Sparrow FieldObject와 동일하게 가져가야 함.

// refactor 
// UI랑 생명주기를 다르게? -> FieldObjectDataBase?

using System;
using UniRx;

public class SparrowData : IModel
{
    #region 1. Fields

    private readonly ReactiveProperty<EAnimalState> _sparrowState = new();

    #endregion

    #region 2. Properties

    public IObservable<EAnimalState> OnSparrowStateChanged => _sparrowState;
    
    #endregion

    #region 3. Constructor

    public SparrowData()
    {
        Initialize();
    }

    private void Initialize()
    {
        _sparrowState.Value = EAnimalState.WALK;
    }


    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void ChangeSparrowState(EAnimalState changedState)
    {
        _sparrowState.Value = changedState;
    }

    public EAnimalState GetSparrowState()
    {
        return _sparrowState.Value;
    }

    #endregion
}

// note
// Animator의 condition과 다르지 않도록 주의
public enum EAnimalState
{
    IDLE = 0,
    WALK = 1,
    FLY = 2,
    EAT = 3,
    ATTACK = 4,
    SPIN = 5,
}