using System;
using System.Xml.Serialization;

[XmlRoot]
public class MyCharacterData : IModel
{
    #region 1. Fields

    private int _name;
    private int _age;
    private int _budget;
    
    [XmlElement(ElementName = "Budget")]
    public int Budget
    {
        get => _budget;
        set => _budget = value;
    }
    private int _routineOneSuccessTime;
    
    #endregion
    #region 2. Properties
    
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    
    [XmlElement(ElementName = "Age")]
    public int Age { get; set; }

    

    public int RoutineSuccessRewardMoney { get; set; }

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