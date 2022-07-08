using DewIt.Model.DataTypes;
using DewIt.Model.Infrastructure;
using DewIt.Model.Processing;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Infrastructure.Bootstrapping;

internal class Bootstrapper : IBootstrapper<DewItState>
{
    private readonly ILogger _logger;
    private readonly IBootstrapData _bootstrapData;

    public Bootstrapper(ILogger logger, IBootstrapData bootstrapData)
    {
        _logger = logger;
        _bootstrapData = bootstrapData ?? throw new ArgumentNullException(nameof(bootstrapData));
    }
    
    public void Bootstrap(DewItState state)
    {
        _logger.Log(LogLevel.Information, "Bootstrapping!");
        
        // set up a list of things to do, and then process it
        var processSteps = SetupBootstrapProcessSteps();
        var result = new Processor().RunSteps(processSteps);
        if (!result.Failed) return;

        // log out the reasons for failure
        var reasons = result.GetFailureReasons();
        _logger.Log(LogLevel.Error, "FAILED BOOTSTRAPPING:");
        reasons.ForEach(it => _logger.Log(LogLevel.Error, it));

        // Failure to bootstrap is deadly: exit the app!
        Environment.Exit(1);
    }

    private IEnumerable<IProcessStep> SetupBootstrapProcessSteps()
    {
        // TODO: parse CLI options
        // TODO: initialize config in app-dir
        // TODO: setup performance marking
        // TODO: setup crash reporter
        // TODO: setup i18n
        
        IProcessStep initializeRepositories = new InitializeRepositoriesStep(_logger, _bootstrapData);
        

        // TODO: find additional bootstrapping steps from plugins

        var steps = new List<IProcessStep>
        {
            initializeRepositories,
        };

        return steps;
    }
}