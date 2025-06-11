public class MyCharacterManager : SingletonBase<MyCharacterManager>, IManager
{
    public MyCharacterManager()
    {
        Budget = 0;
    }

    public int Budget { get; private set; }
}