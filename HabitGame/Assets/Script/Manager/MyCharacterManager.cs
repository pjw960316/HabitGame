/* TODO
 1. 내 캐릭터의 model인 _myCharacterData를 로드하고 이용한다.
 2. _myCharacterData를 이용해서 캐릭터의 동작을 관리한다.
 3. MyCharacterManager 보다 추상화 단계가 낮은 1개의 책임을 수행하는 여러 클래스 (Controller 분해)로 component 채우는. 
*/

public class MyCharacterManager : SingletonBase<MyCharacterManager>, IManager
{
    private MyCharacterData _myCharacterData;
    public MyCharacterManager()
    {
        
    }
}