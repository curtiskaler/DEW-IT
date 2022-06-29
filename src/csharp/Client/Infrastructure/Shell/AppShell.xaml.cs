namespace DewIt.Client.infrastructure.shell;

public partial class AppShell : Shell
{
    public AppShell()
    {
        System.Diagnostics.Debug.WriteLine("AppShell!");
        InitializeComponent();
    }
}
