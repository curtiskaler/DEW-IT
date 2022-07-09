using DewIt.Model.DataTypes;
using DewIt.Model.Infrastructure;
using DewIt.Model.Processing;
using DewIt.Model.Processing.Results;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Infrastructure.Bootstrapping;

internal class LoadConfigFileStep : ProcessStep
{
    private readonly ILogger _logger;
    private readonly IBootstrapperServices _services;

    public override string Title => "Load config file";

    public DewItState State = new();

    public LoadConfigFileStep(ILogger logger, IBootstrapperServices services) 
        : base(logger)
    {
        _logger = logger;
        _services = services;
    }
    
    public override IResult<DewItState> Execute(IProcessResult processResult)
    {
        _logger.Log(LogLevel.Information, ProcessingStrings.ExecutingStep, Title);

        try
        {
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to load config file");
            return ResultFactory.FAILURE<DewItState>(Title, "Failed to load config file", ex);
        }

        return ResultFactory.SUCCESS(ProcessingStrings.Finished, State);
    }
}