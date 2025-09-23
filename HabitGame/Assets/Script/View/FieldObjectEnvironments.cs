// note
// One Script -> Many Concrete Classes

public class FieldObjectRock : FieldObjectEnvironmentBase
{
    protected sealed override void Initialize()
    {
        base.Initialize();
    }

    protected sealed override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.ROCK;
    }
}

public class FieldObjectFlower : FieldObjectEnvironmentBase
{
    protected sealed override void Initialize()
    {
        base.Initialize();
    }

    protected sealed override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.FLOWER;
    }
}

public class FieldObjectMushroom : FieldObjectEnvironmentBase
{
    protected sealed override void Initialize()
    {
        base.Initialize();
    }

    protected sealed override void InitializeEnumFieldObjectKey()
    {
        _eFieldObjectKey = EFieldObject.MUSHROOM;
    }
}