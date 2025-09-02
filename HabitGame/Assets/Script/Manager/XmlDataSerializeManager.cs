using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


// Note
// 책임
// 1. 모든 XML 데이터를 Deserialize 할 책임
// 2. Deserialize 된 XML Data Instance를 각각의 Manager에게 전달.
// 3. Serialize를 통해 XML Data를 업데이트
public partial class XmlDataSerializeManager : ManagerBase<XmlDataSerializeManager>, IManager
{
    #region 1. Fields

    private List<IModel> _modelList;
    private List<XmlFileData> _xmlFileDataList;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
        _modelList = new List<IModel>();
        _xmlFileDataList = new List<XmlFileData>();

        InitializeXmlFileDataList();
        InitializeModelListFromXml();
    }

    public void Initialize()
    {
        //
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void SetModel(IEnumerable<IModel> _list)
    {
    }

    public List<IModel> GetModelListWithDeserializedXml()
    {
        return _modelList;
    }

    private void InitializeModelListFromXml()
    {
        foreach (var xmlFileData in _xmlFileDataList)
        {
            //log
            Debug.Log($"Absolute Path : {xmlFileData.AbsolutePath}");
            
            var xmlType = xmlFileData.DataType;
            var text = GetXmlText(xmlFileData);
            
            var xmlSerializer = new XmlSerializer(xmlType);
            using var stringReader = new StringReader(text);
            ExceptionHelper.CheckNullException(stringReader, "stringReader");

            var model = xmlSerializer.Deserialize(stringReader) as IModel;

            _modelList.Add(model);
        }
    }

    private string GetXmlText(XmlFileData xmlFileData)
    {
        var xmlRelativePath = xmlFileData.RelativePath;
        var xmlAbsolutePath = xmlFileData.AbsolutePath;
        var text = "";

        if (!File.Exists(xmlAbsolutePath))
        {
            text = Resources.Load<TextAsset>(xmlRelativePath).text;

            if (text == null)
            {
                throw new FileLoadException("Resources 폴더에 해당 파일이 없다.");
            }

            File.WriteAllText(xmlAbsolutePath, text);
        }
        else
        {
            text = File.ReadAllText(xmlAbsolutePath);

            if (text == null)
            {
                throw new FileLoadException("절대 경로에 해당 파일이 없다.");
            }
        }

        return text;
    }
    public void SerializeXmlData<TModel>(TModel model) where TModel : IModel
    {
        var serializer = new XmlSerializer(typeof(MyCharacterData));
        var path = _xmlFileDataList[0].AbsolutePath; //test

        using var writer = new StreamWriter(path);

        serializer.Serialize(writer, model);
    }

    #endregion
}