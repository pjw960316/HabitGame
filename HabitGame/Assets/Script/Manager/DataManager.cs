using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// todo
// 1. Data를 로드하고, 다른 매니저에게 전달?

// Note
// 책임
// 1. 모든 XML 데이터를 Deserialize 할 책임
// 2. Deserialize 된 XML Data Instance를 각각의 Manager에게 전달.
// 3. Serialize를 통해 XML Data를 업데이트
public class DataManager : ManagerBase<DataManager>, IManager
{
    #region 1. Fields

    private MyCharacterManager _myCharacterManager;

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        _myCharacterManager = MyCharacterManager.Instance;
    }

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<IModel> _list)
    {
    }

    // fix
    private void DeserializeAllData()
    {
        var test = GetDeserializedXmlData<MyCharacterData>("MyCharacterData");
    }

    public T GetDeserializedXmlData<T>(string resourcePath) where T : class
    {
        var xmlString = GetTextAsset(resourcePath)?.text;
        if (xmlString == null)
        {
            throw new NullReferenceException("xmlString is null");
        }

        var stringReader = new StringReader(xmlString);
        var xmlSerializer = new XmlSerializer(typeof(T));

        return xmlSerializer.Deserialize(stringReader) as T;
    }

    //test
    public void UpdateData()
    {
        SerializeXmlData();
    }

    //test
    private void SerializeXmlData()
    {
        //test

        Debug.Log($"serialize 직전 시점 {_myCharacterManager.MyCharacterData.RoutineSuccessRewardMoney}");
        var serializer = new XmlSerializer(typeof(MyCharacterData));
        var path = Path.Combine(Application.dataPath, "Resources/MyCharacterData.xml");
        
        using (var writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, _myCharacterManager.MyCharacterData);
        }

        

        //test
    }

    // refactor 
    // addressable
    private TextAsset GetTextAsset(string resourcePath)
    {
        var textAsset = Resources.Load<TextAsset>(resourcePath);

        ExceptionHelper.CheckNullException(textAsset, "textAsset");

        return textAsset;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}