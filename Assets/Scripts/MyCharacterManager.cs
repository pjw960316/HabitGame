using UnityEngine;

// 순수 데이터 클래스
public class MyCharacterManager : MonoBehaviour //Mono 없애자 
{
    // 싱글턴 인스턴스를 담는 정적 변수
    private static MyCharacterManager _myCharacterManager;
    private int _budget;

    // 외부에서 접근 가능한 프로퍼티
    public static MyCharacterManager Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성 (옵션)
            if (_myCharacterManager == null)
            {
                _myCharacterManager = FindObjectOfType<MyCharacterManager>(); //이 방식 X
            }
            return _myCharacterManager;
        }
    }

    //얘는 public이면 안 됨
    public void OnAwake()
    {
        GetServerData();
    }

    public void Handle()
    {
        
        
    }
    
    private void GetServerData()
    {
        _budget = 0; //test code
    }

    private void UpdateBudget()
    {
        _budget += 1000; //magic number test
    }

    public int GetBudget()
    {
        return _budget;
    }
}