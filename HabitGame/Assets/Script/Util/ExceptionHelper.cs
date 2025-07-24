using System;

public static class ExceptionHelper
{
    public static void CheckNullException(string argumentName, object instance)
    {
        if (instance == null)
        {
            throw new NullReferenceException($"{argumentName} is null");
        }
    }
}
