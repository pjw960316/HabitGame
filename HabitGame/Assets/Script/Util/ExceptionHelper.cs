using System;

public static class ExceptionHelper
{
    public static void ShowNuException(object instance, string argumentName)
    {
        if (instance == null)
        {
            throw new NullReferenceException($"{argumentName} is null");
        }
    }
    
    public static void CheckNullExceptionWithMessage(object instance, string argumentName, string message)
    {
        if (instance == null)
        {
            throw new NullReferenceException($"{argumentName} is null" + $"\n {message}");
        }
    }
}
