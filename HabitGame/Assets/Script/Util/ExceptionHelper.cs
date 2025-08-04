using System;

public static class ExceptionHelper
{
    //refactor
    //이거 중요한 건 아닌데
    //스크립트 달라서 warning 뜸.
    public static void CheckNullException(object instance, string argumentName)
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
