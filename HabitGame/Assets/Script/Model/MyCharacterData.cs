using System.Xml.Serialization;

[XmlRoot]
public class MyCharacterData : IModel
{
    #region 2. Properties

    public string Name { get; set; }
    public int RoutineOneSuccessTime { get; set; }
    
    public int Budget { get; set; }

    #endregion

    #region 3. Constructor

    // default

    #endregion

    #region 4. Methods

    // default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}