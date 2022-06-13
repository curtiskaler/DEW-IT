using DewIt.Client.view.shell;

namespace DewIt.Client;

public partial class DewItApp : Application
{
	public DewItApp()
	{
        System.Diagnostics.Debug.WriteLine("App!");

        InitializeComponent();

		MainPage = new AppShell();
	}
}
