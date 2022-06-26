using DewIt.Client.Dev;
using DewIt.Client.infrastructure.shell;
using DewIt.Model.DataTypes;
using DewIt.Model.Infrastructure;

namespace DewIt.Client.infrastructure;

public partial class DewItClient : Application
{
    internal static DewItState State { get; } = DewItState.UNINITIALIZED;

	public DewItClient()
	{
        // This is the entry-point for the application.
        // Initialize the object graph (dependencies) as much as possible RIGHT HERE.
        
        System.Diagnostics.Debug.WriteLine("App!");
        InitializeComponent();

        // TODO: Update this to a bootstrapper with an actual backend
        IBootstrapper<DewItState> bootstrapper = new DevBootstrapper();

        State.Initialize(bootstrapper);
		
        MainPage = new AppShell();
	}
}
