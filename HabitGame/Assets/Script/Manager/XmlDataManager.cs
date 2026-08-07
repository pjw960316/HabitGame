using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// NOTE
// 1. 모든 XML 데이터를 Deserialize 할 책임
// 2. Deserialize 된 XML Data Instance를 각각의 Manager에게 전달.
// 3. Serialize를 통해 XML Data를 업데이트
public class XmlDataManager : ManagerBase<XmlDataManager>
{
    public class XmlFileData
    {
        public Type DataType;
        public string ResourcesRelativePath;
        public string PersistentFilePath;
    }

    #region 1. Fields

    private List<object> _deserializedXmlList;
    private List<XmlFileData> _xmlFileDataList;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void PreInitialize()
    {
        _deserializedXmlList = new List<object>();
        _xmlFileDataList = new List<XmlFileData>();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    // 


    public void RegisterDeserializedXmlData()
    {
        _xmlFileDataList.Add(new XmlFileData
        {
            DataType = typeof(MyCharacterData),
            ResourcesRelativePath = "XML/MyCharacterData",

            // WARNING
            // 최초 한 번은 Resources에 접근해서 파일을 읽어야
            // Application.persistentDataPath (플랫폼 상관 없는 주소) -> 에서 복사된 파일로 읽는다.
            PersistentFilePath = Application.persistentDataPath + "/MyCharacterData.xml"
        });
        
        foreach (var xmlFileData in _xmlFileDataList)
        {
            //log
            Debug.Log($"Persistent File Path : {xmlFileData.PersistentFilePath}");

            var xmlType = xmlFileData.DataType;
            var text = GetXmlText(xmlFileData);

            var xmlSerializer = new XmlSerializer(xmlType);
            using var stringReader = new StringReader(text);
            ExceptionHelper.CheckNullException(stringReader, "stringReader");
            
            _deserializedXmlList.Add(xmlSerializer.Deserialize(stringReader));
        }
    }
    
    public T GetDeserializedXmlData<T>() 
    {
        foreach (var deserializedXml in _deserializedXmlList)
        {
            if (deserializedXml is T targetDeserializedXml)
            {
                return targetDeserializedXml;
            }
        }

        throw new NullReferenceException("GetDeserializedXml Fail");
    }

    public int GetDeserializedXmlListCount()
    {
        return _xmlFileDataList.Count;
    }
    
    private string GetXmlText(XmlFileData xmlFileData)
    {
        var resourcesRelativePath = xmlFileData.ResourcesRelativePath;
        var persistentFilePath = xmlFileData.PersistentFilePath;
        var text = "";

        if (!File.Exists(persistentFilePath))
        {
            text = Resources.Load<TextAsset>(resourcesRelativePath).text;

            if (text == null)
            {
                throw new FileLoadException("Resources 폴더에 해당 파일이 없다.");
            }

            File.WriteAllText(persistentFilePath, text);
        }
        else
        {
            text = File.ReadAllText(persistentFilePath);

            if (text == null)
            {
                throw new FileLoadException("절대 경로에 해당 파일이 없다.");
            }
        }

        return text;
    }

    public void SerializeXmlData<TModel>(TModel model)
    {
        var serializer = new XmlSerializer(typeof(MyCharacterData));
        
        // TEST
        // 테스트 상태의 path다.
        var path = _xmlFileDataList[0].PersistentFilePath;

        using var writer = new StreamWriter(path);

        serializer.Serialize(writer, model);
    }

    #endregion
}
