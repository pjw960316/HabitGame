using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * TODO:
 * loadScene에서 모든 동작을 완료했을 때 -> MainScene을 진입하도록
 * 모든 싱글턴을 초기화 시키기
 * 이건 Mono가 상속이 필요한가?
 */
public class GameStartManager : MonoBehaviour
{
    private void Awake()
    {
        SingletonInitializeManager singletonInitializeManager = new SingletonInitializeManager();
        
        ChangeSceneManager changeSceneManager = new ChangeSceneManager();
        changeSceneManager.ChangeScene();
        
        DontDestroyOnLoad(this); //TODO : Should need? 너의 책임은 시작시키고 죽는건데?
    }
}

public class SingletonInitializeManager
{
    public SingletonInitializeManager()
    {
        // TODO : Singleton을 여기서 항상 초기화 -> 매 번 하는 게 귀찮고 유지보수가 불가능 (회사에서도...)
        // TODO : singleton의 초기화 순서 여부에 따라 null이 나올 수 있음. 답 없는 문제임
        MyCharacterManager myCharacterManager = new MyCharacterManager();
    }
}

public class ChangeSceneManager
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Main");
    }
}