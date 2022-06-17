using DewIt.Client.infrastructure.shell;

namespace DewIt.Client.infrastructure;

public partial class DewItApp : Application
{
	public DewItApp()
	{
        System.Diagnostics.Debug.WriteLine("App!");

        InitializeComponent();

		MainPage = new AppShell();
	}
}
