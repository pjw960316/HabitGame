using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// Note
// 책임
// 1. 모든 XML 데이터를 Deserialize 할 책임
// 2. Deserialize 된 XML Data Instance를 각각의 Manager에게 전달.
// 3. Serialize를 통해 XML Data를 업데이트
public class XmlDataSerializeManager : ManagerBase<XmlDataSerializeManager>, IManager
{
    #region 1. Fields
    
    private Dictionary<Type, string> _xmlFullPathDictionary;
    private List<IModel> _modelList;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
        _xmlFullPathDictionary = new Dictionary<Type, string>();
        _modelList = new List<IModel>();

        Test1();
        
        //SetXmlFullPath();
    }

    public void Initialize()
    {
        //
        
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<IModel> _list)
    {
    }

    private void SetXmlFullPath()
    {
        //test
        var path = Path.Combine(Application.dataPath, "Resources/MyCharacterData.xml");
        _xmlFullPathDictionary.Add(typeof(MyCharacterData), path);
    }

    private void SetModelListWithDeserializedXml()
    {
        foreach (var element in _xmlFullPathDictionary)
        {
            var xmlType = element.Key;
            var xmlPath = element.Value;

            var xmlText = GetAllText(xmlPath);
            var xmlSerializer = new XmlSerializer(xmlType);
            var stringReader = new StringReader(xmlText);
            ExceptionHelper.CheckNullException(stringReader, "stringReader");

            var model = xmlSerializer.Deserialize(stringReader) as IModel;

            _modelList.Add(model);
        }
    }

    public List<IModel> GetModelListWithDeserializedXml()
    {
        return _modelList;
    }
    private void Test1()
    {
        if (!File.Exists(Application.persistentDataPath + "/MyCharacterData.xml"))
        {
            Debug.Log("없어");
            var textAsset = Resources.Load<TextAsset>("MyCharacterData").text;

            File.WriteAllText(Application.persistentDataPath + "/MyCharacterData.xml", textAsset);
        }
        else
        {
            Debug.Log("있어");
            SetXmlFullPath();
            SetModelListWithDeserializedXml();
        }
        
        Debug.Log($"{Application.persistentDataPath + "/MyCharacterData.xml"}");
    }
    private string GetAllText(string resourceFullPath)
    {
        var text = File.ReadAllText(resourceFullPath);

        if (text == null)
        {
            throw new FileLoadException("Read Xml fail");
        }

        return text;
    }

    public void SerializeXmlData<TModel>(TModel model) where TModel : IModel
    {
        var serializer = new XmlSerializer(typeof(MyCharacterData));
        var path = _xmlFullPathDictionary[typeof(MyCharacterData)];

        using var writer = new StreamWriter(path);

        serializer.Serialize(writer, model);
    }

    // refactor 
    // addressable
    // Resources.Load는 Runtime에 갱신된 xml 데이터를 반영하지 못한다.
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