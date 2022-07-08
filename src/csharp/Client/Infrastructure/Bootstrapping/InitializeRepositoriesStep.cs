using DewIt.Model.Infrastructure;
using DewIt.Model.Persistence;
using DewIt.Model.Processing;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Infrastructure.Bootstrapping;

internal class InitializeRepositoriesStep : ProcessStep
{
    private readonly IRepositoryCollection _repositories;
    private readonly ILogger _logger;

    public InitializeRepositoriesStep(ILogger logger, IBootstrapData config)
    {
        _logger = logger;
        _repositories = config.Repositories;
    }

    public override IResult Execute(IStepAndResultCollection previousSteps)
    {
        _logger.LogTrace(Strings.BootstrappingRepositories);

        try
        {
            _repositories.Initialize();
        }
        catch (Exception ex)
        {
            _logger.LogError(Strings.BootstrappingRepositories);
            return ResultFactory.FAILURE(Title, Strings.ERROR_CouldNotInitRepos, ex);
        }

        return ResultFactory.SUCCESS(Strings.Finished);
    }

    public override string Title => Strings.BootstrapRepositories;
}