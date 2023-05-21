using System;
using B7.Processing;
using DewIt.Client.Persistence;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Bootstrapping;

internal class InitializeRepositoriesStep : ProcessStep
{
    private readonly IRepositoryCollection _repositories;

    public InitializeRepositoriesStep(ILogger logger, IBootstrapperServices services) : base(logger)
    {
        _repositories = services?.Repositories ?? throw new ArgumentNullException(nameof(_repositories));
    }

    public override string Title => BootstrappingStrings.BootstrapRepositories;

    public override IResult Execute(IProcessResult resultsOfPreviousSteps)
    {
        _logger.Log(LogLevel.Information, ProcessingStrings.msg_ExecutingStep, Title);

        try
        {
            _repositories.Initialize();
        }
        catch (Exception ex)
        {
            string msg = BootstrappingStrings.ERROR_CouldNotInitRepos;
            _logger.LogError(msg);
            _logger.LogError(ex.Message);
            return ResultFactory.FAILURE<ClientState>(Title, msg, ex);
        }

        return ResultFactory.SUCCESS(ProcessingStrings.msg_Finished);
    }
}

