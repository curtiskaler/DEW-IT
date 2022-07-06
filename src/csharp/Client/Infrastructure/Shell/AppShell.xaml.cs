namespace DewIt.Client.infrastructure.shell;

public partial class AppShell : Shell
{
    public AppShell()
    {
        System.Diagnostics.Debug.WriteLine("Entering AppShell ctor!");
        InitializeComponent();
        System.Diagnostics.Debug.WriteLine("Leaving AppShell ctor!");
    }
}
