namespace Not_Mono
{
    public class MyCharacterManager
    {
        // field
        private static MyCharacterManager _myCharacterManager;

        // Property
        public static MyCharacterManager Instance
        {
            get
            {
                if (_myCharacterManager == null) _myCharacterManager = new MyCharacterManager();

                return _myCharacterManager;
            }
        }

        public int Budget { get; private set; }

        private MyCharacterManager()
        {
            Budget = 0;
        }
    }
}