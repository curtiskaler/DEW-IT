using DewIt.Model.CommandLine;
using DewIt.Model.DataTypes;
using DewIt.Model.Infrastructure;
using DewIt.Model.Processing;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Infrastructure.Bootstrapping;

internal class Bootstrapper : IBootstrapper<DewItState>
{
    private readonly ILogger _logger;
    private readonly IProcessor _processor;
    private readonly IBootstrapperServices _services;

    public Bootstrapper(ILogger logger, IProcessor processor, IBootstrapperServices services)
    {
        _logger = logger;
        _processor = processor;
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }
    
    public DewItState Bootstrap()
    {
        _logger.Log(LogLevel.Information, "Bootstrapping!");
        
        // set up a list of things to do, and then process it
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

        
        var steps = new List<IProcessStep>
        {
            new LoadConfigFileStep(_logger, _services),
            new InitializeRepositoriesStep(_logger, _services),
        };

        // TODO: find additional bootstrapping steps from plugins


        return steps;
    }

    private void DieIfBootstrappingFailed(IProcessResult result)
    {
        if (!result.Failed) return;

        // log out the reasons for failure
        var reasons = result.GetFailureReasons();

        _logger.Log(LogLevel.Error, "FAILED BOOTSTRAPPING:");
        reasons.ForEach(it => _logger.Log(LogLevel.Error, it, Array.Empty<object>()));

        // Failure to bootstrap is deadly: exit the app!
        Environment.Exit(1);
    }

    private static DewItState ResolveConfig(IStepAndResultCollection stepsAndResults)
    {
        // TODO: Iterate over the process results and build the application state.
        //DEMO.Start1();

        return new DewItState();
    }
}