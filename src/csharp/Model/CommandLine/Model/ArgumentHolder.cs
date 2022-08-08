namespace DewIt.Model.CommandLine;

public abstract class ArgumentHolder<T> where T : ArgumentHolder<T>
{
    public List<Argument> Arguments { get; set; }

    protected ArgumentHolder()
    {
        Arguments = new List<Argument>();
    }

    public T WithArgument()
    {
        return WithArgument("", "", false, Arity.ONE);
    }

    public T WithArgument(string token, string description)
    {
        return WithArgument(token, description, false, Arity.ONE);
    }

    public T WithArgument(string token, string description, bool isRequired)
    {
        return WithArgument(token, description, isRequired, Arity.ONE);
    }

    public T WithArgument(string token, string description, bool isRequired, Arity arity)
    {
        return WithArgument(new Argument(token, description, isRequired, arity));
    }

    public T WithArgument(Argument argument)
    {
        Arguments.Add(argument);
        return (T)this;
    }

    public void Add(Argument argument)
    {
        Arguments.Add(argument);
    }

    public Argument AddArgument(string token, string description)
    {
        return AddArgument(token, description, false, Arity.ONE);
    }

    public Argument AddArgument(string token, string description, bool isRequired)
    {
        return AddArgument(token, description, isRequired, Arity.ONE);
    }

    public Argument AddArgument(string token, string description, bool isRequired, Arity arity)
    {
        return AddArgument(new Argument(token, description, isRequired, arity));
    }

    public Argument AddArgument(Argument argument)
    {
        Arguments.Add(argument);
        return argument;
    }
}
