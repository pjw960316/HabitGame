using System;
using NUnit.Framework;

public class InputManagerExtensibilityTests
{
    [Test]
    public void OnTouchedFieldObject_ExposesBaseFieldObjectContract()
    {
        // @AIAD-4STEP: This is a light extensibility guard, not a full behavior test.
        // Future selection logic can move from FieldObjectSparrow to any FieldObjectBase
        // without forcing subscribers to depend on a concrete animal class.
        var property = typeof(InputManager).GetProperty(nameof(InputManager.OnTouchedFieldObject));

        Assert.That(property, Is.Not.Null);
        Assert.That(property.PropertyType, Is.EqualTo(typeof(IObservable<FieldObjectBase>)));
    }
}
