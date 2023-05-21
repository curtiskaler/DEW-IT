using System;
using B7.Processing;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Bootstrapping;

internal class LoadConfigFileStep : ProcessStep
{
    private readonly IBootstrapperServices _services;

    public override string Title => BootstrappingStrings.LoadConfiguration;

    public ClientState State = new();

    public LoadConfigFileStep(ILogger logger, IBootstrapperServices services) : base(logger)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public override IResult Execute(IProcessResult resultsOfPreviousSteps)
    {
        _logger.Log(LogLevel.Information, ProcessingStrings.msg_ExecutingStep, Title);

        try
        {

        }
        catch (Exception ex)
        {
            string msg = BootstrappingStrings.ERROR_CouldNotLoadConfiguration;
            _logger.LogError(msg);
            return ResultFactory.FAILURE<ClientState>(Title, msg, ex);
        }

        return ResultFactory.SUCCESS(ProcessingStrings.msg_Finished, State);
    }
}

