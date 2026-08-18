using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// NOTE : 책임
// 1. Resources에 존재하는 XML 파일들의 데이터들을 Deserialize 할 책임
// 2. Deserialize 성공한 데이터를 클래스 메모리에 로드
// 3. Serialize를 통해 XML Data를 업데이트
public class XmlDataManager : ManagerBase<XmlDataManager>
{
    // NOTE : XML 파일과 해당 DTO는 일대일 대응 
    public class XmlFileData
    {
        // WARNING : 두 가지 주소
        // 최초 한 번은 ResourcesRelativePath(=Resources 폴더 기반) 접근해서 파일을 읽는다.
        // Application.persistentDataPath(=플랫폼 상관 없는 주소) -> 에서 복사된 파일로 읽는다.
        public string ResourcesRelativePath;
        public string PersistentFilePath;
    }

    #region 1. Fields

    private Dictionary<Type, object> _deserializedXmlDictionary;
    private Dictionary<Type, XmlFileData> _xmlFileDataDictionary;

    #endregion

    #region 2. Properties

    // 

    #endregion

    #region 3. Constructor

    public sealed override void PreInitialize()
    {
        _deserializedXmlDictionary = new Dictionary<Type, object>();
        _xmlFileDataDictionary = new Dictionary<Type, XmlFileData>();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods
    public void RegisterDeserializedXmlData()
    {
        _xmlFileDataDictionary.Add(typeof(MyCharacterData), new XmlFileData
        {
            ResourcesRelativePath = "XML/MyCharacterData",
            PersistentFilePath = Application.persistentDataPath + "/MyCharacterData.xml"
        });
        
        foreach (var xmlFileDataPair in _xmlFileDataDictionary)
        {
            var xmlType = xmlFileDataPair.Key;
            var xmlFileData = xmlFileDataPair.Value;
            var text = GetXmlText(xmlFileData);
            var xmlSerializer = new XmlSerializer(xmlType);
            
            using var stringReader = new StringReader(text);
            ExceptionHelper.CheckNullException(stringReader, "stringReader");
            
            _deserializedXmlDictionary.Add(xmlType, xmlSerializer.Deserialize(stringReader));
        }
    }
    
    // NOTE
    // 다른 매니저에서 XML 데이터에서 알맞은 타입의 데이터를 로드하도록 제공한다.
    public T GetDeserializedXmlData<T>() 
    {
        if (_deserializedXmlDictionary.TryGetValue(typeof(T), out var deserializedXml) && deserializedXml is T targetDeserializedXml)
        {
            return targetDeserializedXml;
        }

        throw new NullReferenceException("GetDeserializedXml Fail");
    }

    public int GetDeserializedXmlListCount()
    {
        return _xmlFileDataDictionary.Count;
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

    public void SerializeXmlData<T>(T dataType)
    {
        var xmlFileData = GetXmlFileData(typeof(T));
        var serializer = new XmlSerializer(typeof(T));
        var path = xmlFileData.PersistentFilePath;

        using var writer = new StreamWriter(path);

        serializer.Serialize(writer, dataType);
    }

    private XmlFileData GetXmlFileData(Type dataType)
    {
        if (_xmlFileDataDictionary.TryGetValue(dataType, out var xmlFileData))
        {
            return xmlFileData;
        }

        throw new InvalidOperationException($"등록되지 않은 XML 데이터 타입입니다. Type : {dataType.Name}");
    }

    #endregion
}
