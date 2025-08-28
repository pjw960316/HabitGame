using System;
using UnityEngine;

public partial class XmlDataSerializeManager : ManagerBase<XmlDataSerializeManager>, IManager
{
    public class XmlFileData
    {
        public Type DataType;

        // note : Resources.Load<>
        public string RelativePath;

        // note : File Read I/O
        public string AbsolutePath;
    }

    private void InitializeXmlFileDataList()
    {
        _xmlFileDataList.Add(new XmlFileData
        {
            DataType = typeof(MyCharacterData),
            RelativePath = "MyCharacterData",
            AbsolutePath = Application.persistentDataPath + "/MyCharacterData.xml"
        });
    }
}