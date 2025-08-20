// refactor
// RoutineCheckPresenter와 함께 하려 했으나, presenter가 2개의 view를 책임지는 건 아니라 판단.
// 그래서 두 개를 일단 분리해서 구현
// 하지만 왠지 두 개의 공통 기능이 생길 것 같음 -> 상위로 묶는 방식
// 이거에 대한 판단이 아직 서지 않는다.

using System.Collections.Immutable;
using System.Linq;

public class RoutineRecordPresenter : PresenterBase
{
    #region 1. Fields

    private const int ROUTINE_RECORD_COUNT = 30;
    
    private UIRoutineRecordPopup _uiRoutineRecordPopup;

    private MyCharacterManager _myCharacterManager;
    private ImmutableDictionary<string, ImmutableList<bool>> _routineRecordDictionary;
    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    // refactor
    // Initialize()안에 막 때려 넣는 행위?
    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _myCharacterManager = MyCharacterManager.Instance;
        
        _uiRoutineRecordPopup = _view as UIRoutineRecordPopup;
        ExceptionHelper.CheckNullException(_uiRoutineRecordPopup, "_uiRoutineRecordPopup");
        
        UpdateCurrentRoutineRecords();
    }

    #endregion

    #region 4. Methods

    // default

    #endregion

    #region 5. Request Methods

    // default

    #endregion

    #region 6. EventHandlers

    private void UpdateCurrentRoutineRecords()
    {
        _routineRecordDictionary = _myCharacterManager.GetRoutineRecordDictionary();
        
        _uiRoutineRecordPopup.UpdateRoutineRecord(_routineRecordDictionary);
    }
    

    #endregion
}
