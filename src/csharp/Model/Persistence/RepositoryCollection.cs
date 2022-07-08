using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DewIt.Model.Persistence
{
    public interface IRepositoryCollection
    {
        ILaneRepository Lanes { get; }
        ICardRepository Cards { get; }

        void Initialize();
    }

    public class RepositoryCollection : IRepositoryCollection
    {
        private ILogger _logger;
        public ILaneRepository Lanes { get; }
        public ICardRepository Cards { get; }

        public RepositoryCollection(ILogger logger, ILaneRepository lanes, ICardRepository cards)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Lanes = lanes ?? throw new ArgumentNullException(nameof(lanes));
            Cards = cards ?? throw new ArgumentNullException(nameof(cards));
        }

        public void Initialize()
        {
            _logger.Log(LogLevel.Information, "Initializing repositories!");
            this.Cards.Initialize();
            this.Lanes.Initialize();
        }
    }
}