using DewIt.Client.infrastructure.shell;
using DewIt.Model.DataTypes;

namespace DewIt.Client.infrastructure;

public partial class DewItClient : Application
{
    internal static DewItState State { get; private set; }

	public DewItClient(DewItState state)
	{
        // This is the ctor for the Client.
        // The object graph (dependencies) were initialized in EntryPoint.cs
        State = state ?? throw new ArgumentNullException(nameof(state));

        State.Initialize();

        InitializeComponent();
        MainPage = new AppShell();
	}
}
