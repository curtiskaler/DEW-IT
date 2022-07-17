namespace DewIt.Model.CommandLine;

public class DEMO
{
    public static void Start()
    {
        var commandLine = new CommandLineBuilder("Pizza Ordering System")
            .AddOption("d", "debug", "output extra debugging")
            .AddOption("p", "pizza-type", "flavour of pizza")
            .WithArgument("type", "flavour of pizza")
            .AddOption("s", "small", "small pizza slice");

        var parsedCommandLineOptions = commandLine.Parse();
    }
}