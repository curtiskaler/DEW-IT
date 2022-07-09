using DewIt.Client.infrastructure.shell;
using DewIt.Model.DataTypes;
using DewIt.Model.Infrastructure;

namespace DewIt.Client.infrastructure;

public partial class DewItClient
{
    internal static DewItState State { get; private set; }

	public DewItClient(IBootstrapper<DewItState> bootstrapper)
	{
        State = bootstrapper.Bootstrap();
        InitializeComponent();
        MainPage = new AppShell();
	}
}
