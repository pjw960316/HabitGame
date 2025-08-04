using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// todo
// 1. Data를 로드하고, 다른 매니저에게 전달?

// Note
// 책임
// 1. XML에서 데이터를 로드할 책임
// 2. XML에서 로드한 데이터를 다른 Manager에게 전달해서 Instance 생성
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
    
    //default

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        DeserializeAllData();
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<ScriptableObject> _list)
    {
    }

    private void DeserializeAllData()
    {
        Fruits test = GetDeserializedXmlData<Fruits>("MyCharacterData");
    }

    private T GetDeserializedXmlData<T>(string resourcePath) where T : class
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