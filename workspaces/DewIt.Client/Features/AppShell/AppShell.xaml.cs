namespace DewIt.Client.Features.AppShell;

public partial class AppShell : Shell
{
    public AppShell()
    {
        System.Diagnostics.Debug.WriteLine("AppShell ctor!");
        InitializeComponent();
    }
}

