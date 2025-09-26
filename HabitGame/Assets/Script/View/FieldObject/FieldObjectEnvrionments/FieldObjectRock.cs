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