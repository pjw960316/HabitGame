using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DataManager : ManagerBase<DataManager>, IManager
{
    //test

    public class Fruits
    {
        [XmlElement("Fruit")]
        public List<Fruit> list;
    }

    public class Fruit
    {
        public string Name;
        public int Count;
    }

    #region 1. Fields

    // default

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        Test();
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<ScriptableObject> _list)
    {
    }

    private void Test()
    {
        Debug.Log("DataManager Test Code STart");

        var xmlSerializer = new XmlSerializer(typeof(Fruits));

        //fix
        //주소 이렇게 하면 다른 곳에서 불가능
        
        //refactor
        //addressable을 이용해서 읽고 메모리에서 없애는?
        var streamReader = new StreamReader("C:/HabitGame/HabitGame/Assets/Resources/MyCharacterData.xml");

        var fruits = xmlSerializer.Deserialize(streamReader) as Fruits;

        foreach (var i in fruits.list)
        {
            Debug.Log($"{i.Name} {i.Count}");
        }
    }

    //test

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}