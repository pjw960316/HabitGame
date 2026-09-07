using UnityEngine;

// NOTE : 설계 방향 및 책임
/*
 1. Widget은 반드시 상위 UI View 객체에 붙어서 이용된다.
 2. 자신을 들고 있는 상위 UI View 객체를 몰라서 재사용에 자유롭다.
    - 재사용성이 없는 widget는 귀찮다.  ->  CODEX 시대니까 상관 없나?
 3.모든 Widget은 UIWidgetBase를 상속 받기 때문에 is-A 관계를 만족시키는 중요 메서드만 관리한다.
    - 그 아래에 버튼, 이미지, 뭐 여러 위젯들이 생성
*/    
public abstract class UIWidgetBase : MonoBehaviour, IView
{
    #region 1. Fields

    // default

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
        //
    }

    #endregion

    #region 4. Methods

    // default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}