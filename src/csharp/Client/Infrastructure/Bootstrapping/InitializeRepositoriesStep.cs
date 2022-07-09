using DewIt.Model.Infrastructure;
using DewIt.Model.Persistence;
using DewIt.Model.Processing;
using DewIt.Model.Processing.Results;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Infrastructure.Bootstrapping;

internal class InitializeRepositoriesStep : ProcessStep
{
    private readonly IRepositoryCollection _repositories;
    private readonly ILogger _logger;

    public InitializeRepositoriesStep(ILogger logger, IBootstrapperServices services):base(logger)
    {
        _logger = logger;
        _repositories = services.Repositories;
    }

    public override IResult Execute(IProcessResult processResult)
    {
        _logger.Log(LogLevel.Information, ProcessingStrings.ExecutingStep, Title);

        try
        {
            _repositories.Initialize();
        }
        catch (Exception ex)
        {
            _logger.LogError(Strings.BootstrappingRepositories);
            return ResultFactory.FAILURE(Title, Strings.ERROR_CouldNotInitRepos, ex);
        }

        return ResultFactory.SUCCESS(ProcessingStrings.Finished);
    }

    public override string Title => Strings.BootstrapRepositories;
    
}