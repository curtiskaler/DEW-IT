
using B7.Lifecycle;
using B7.Processing;
using DewIt.Client.Model;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Bootstrapping;

public class Bootstrapper : IBootstrapper<ClientState>
{
    private readonly ILogger _logger;
    private readonly IProcessor _processor;
    private readonly IBootstrapperServices _services;

    public Bootstrapper(ILogger logger, IProcessor processor, IBootstrapperServices services)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public ClientState Bootstrap()
    {
        _logger.Log(LogLevel.Information, "bootstrapping!");

        // set up a list of things to do, and then process it.
        var processSteps = SetupBootstrapProcessSteps();
        var result = _processor.RunSteps(processSteps);
        DieIfBootstrappingFailed(result);

        var config = ResolveConfig(result.StepsAndResults);
        return config;
    }

    private IEnumerable<IProcessStep> SetupBootstrapProcessSteps()
    {
        // TODO: parse CLI options
        // TODO: initialize config in app-dir
        // TODO: setup performance marking
        // TODO: setup crash reporter
        // TODO: setup i18n

        _logger.Log(LogLevel.Debug, "Identifying bootstrap steps!");

        var steps = new List<IProcessStep>()
        {
            new LoadConfigFileStep(_logger, _services),
            new InitializeRepositoriesStep(_logger, _services)
        };


        // TODO: find additional bootstrapping steps from plugins.

        _logger.Log(LogLevel.Debug, "bootstrap steps identified!");

        return steps;
    }

    private void DieIfBootstrappingFailed(IProcessResult result)
    {
        if (!result.Failed) return;

        // log out the reason for the failure.
        var reasons = result.GetFailureReasons();
        _logger.Log(LogLevel.Error, "FAILED BOOTSTRAPPING:");
        reasons.ForEach(it => _logger.Log(LogLevel.Error, it, Array.Empty<object>()));

        // Failure to bootstrap is deadly: exit the app!
        Environment.Exit(1);
    }

    private static ClientState ResolveConfig(IStepAndResultCollection stepsAndResults)
    {
        ClientState state = new ClientState();

        // TODO: Iterate over the process results and build up the application state.

        return state;
    }
}

