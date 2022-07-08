namespace DewIt.Model;

public static class ExceptionExtensions
{
    public static List<Exception> ToList(this Exception ex)
    {
        return new List<Exception>{ex};
    }
}