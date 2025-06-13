using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * TODO:
 * loadScene에서 모든 동작을 완료했을 때 -> MainScene을 진입하도록
 * 모든 싱글턴을 초기화 시키기
 * 이건 Mono가 상속이 필요한가?
 */

/*
 * TODO:
 * GameStartMAnager에서 싱글턴을 모두 초기화 하는 걸 Awake랑 Start로 나누었다면
 * Awake와 Start에서 정확히 어떤 걸 초기화하면 좋을 지 연습해서 Personal Coding Style에 적자.
 *  예를 들면 Binding 같은건.
 */

public class GameStartManager : MonoBehaviour
{
    private void Awake()
    {
        var changeSceneManager = new ChangeSceneManager();
        changeSceneManager.ChangeScene();

        DontDestroyOnLoad(this); //TODO : Should need? 너의 책임은 시작시키고 죽는건데?
    }

    // TODO : 여기서 scriptableObject 들을 연결 후 -> 각각의 Manager들에 전달한다.
    private class ScriptableObjectLoader
    {
        
    }
}


public class ChangeSceneManager
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Main");
    }
}