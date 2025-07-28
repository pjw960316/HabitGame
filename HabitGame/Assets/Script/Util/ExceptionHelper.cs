using System;

public static class ExceptionHelper
{
    public static void CheckNullException(object instance, string argumentName)
    {
        if (instance == null)
        {
            throw new NullReferenceException($"{argumentName} is null");
        }
    }
}
