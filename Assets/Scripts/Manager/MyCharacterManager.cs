/* TODO
 1. 내 캐릭터가 들고 있어야 할 정보.
 
 */
public class MyCharacterManager : SingletonBase<MyCharacterManager>, IManager
{
    public MyCharacterManager()
    {
        Budget = 0;
    }

    public int Budget { get; private set; }
}