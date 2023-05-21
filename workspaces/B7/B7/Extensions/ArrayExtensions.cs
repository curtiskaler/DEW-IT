namespace B7;

public static class ArrayExtensions
{
    public static T[] Concat<T>(this T[] x, T[] y)
    {
        if (x == null) throw new ArgumentNullException(nameof(x));
        if (y == null) throw new ArgumentNullException(nameof(y));
        int oldLen = x.Length;
        Array.Resize<T>(ref x, x.Length + y.Length);
        Array.Copy(y, 0, x, oldLen, y.Length);
        return x;
    }

    public static T[] Add<T>(this T[] x, T y)
    {
        if (x == null) throw new ArgumentNullException(nameof(x));
        if (y == null) throw new ArgumentNullException(nameof(y));
        int oldLen = x.Length;
        Array.Resize<T>(ref x, x.Length + 1);
        x[oldLen] = y;
        return x;
    }
}
