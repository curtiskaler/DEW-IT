using B7.Lifecycle;
using DewIt.Client.Bootstrapping;
using DewIt.Client.Features.AppShell;
using DewIt.Client.Model;

namespace DewIt.Client;

public partial class DewItClient : Application
{
    internal static ClientState State { get; private set; }

    public DewItClient(IBootstrapper<ClientState> bootstrapper)
    {
        System.Diagnostics.Debug.WriteLine("DewItClient ctor!");

        State = bootstrapper.Bootstrap();
        InitializeComponent();
        MainPage = new AppShell();
    }
}

