namespace DewIt.Model.CommandLine;

[Flags]
public enum Arity
{
    ZERO = 1 << 0, // can't start at 0 if we intend to do bitwise checking
    ONE = 1 << 1,
    MANY = 1 << 2
}

public static class ArityExtensions
{
    public static string GetUMLString(this Arity arity)
    {
        int minimum, maximum;

        switch (arity)
        {
            case Arity.ZERO:
                minimum = maximum = 0;
                break;
            case Arity.ONE:
                minimum = maximum = 1;
                break;
            case Arity.MANY:
                minimum = maximum = -1;
                break;
            case Arity.ZERO | Arity.ONE:
                minimum = 0;
                maximum = 1;
                break;
            case Arity.ZERO | Arity.MANY:
                minimum = 0;
                maximum = -1;
                break;
            case Arity.ONE | Arity.MANY:
                minimum = 1;
                maximum = -1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(arity), arity, null);
        }

        string Stringify(int value) => (value == -1) ? "*" : $"{value}";
        var minString = Stringify(minimum);
        var maxString = Stringify(maximum);

        var result = (minimum == maximum) ? minString : $"{minString}..{maxString}";

        return result;
    }
}