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
public class DataManager : ManagerBase<DataManager>, IManager
{
    #region 1. Fields
    
    //default

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        //DeserializeAllData();
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<IModel> _list)
    {
    }

    private void DeserializeAllData()
    {
        MyCharacterData test = GetDeserializedXmlData<MyCharacterData>("MyCharacterData");
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

    public void UpdateData()
    {
        SerializeXmlData();
    }
    
    private void SerializeXmlData()
    {
        var serializer = new XmlSerializer(typeof(MyCharacterData));
        using var writer = new StreamWriter("MyCharacterData.xml");
        //serializer.Serialize(writer, MyCharacterManager.Instance.);
    }
    
    // refactor 
    // addressable
    private TextAsset GetTextAsset(string resourcePath)
    {
        var textAsset = Resources.Load<TextAsset>(resourcePath);
        
        ExceptionHelper.CheckNullException(textAsset, "textAsset");

        return textAsset;
    }
    
    

    //test

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}