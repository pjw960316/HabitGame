using System.Collections.Generic;

public class MyCharacterManager : ManagerBase<MyCharacterManager>, IManager
{
    private MyCharacterData _myCharacterData;

    public void Initialize()
    {
    }

    public void SetModel(IEnumerable<IModel> models)
    {
        foreach (var model in models)
        {
            if (model is MyCharacterData myCharacterData)
            {
                _myCharacterData = myCharacterData;
                
                return;
            }
        }
    }

    //test
    public string GetName()
    {
        return _myCharacterData.name;
    }
}