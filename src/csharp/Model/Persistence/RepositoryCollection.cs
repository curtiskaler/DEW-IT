using System.Diagnostics;

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
        public ILaneRepository Lanes { get; }
        public ICardRepository Cards { get; }

        public RepositoryCollection(ILaneRepository lanes, ICardRepository cards)
        {
            this.Lanes = lanes ?? throw new ArgumentNullException(nameof(lanes));
            this.Cards = cards ?? throw new ArgumentNullException(nameof(cards));
        }

        public void Initialize()
        {
            Debug.WriteLine("RepositoryCollection.Initialize() : Initializing repositories!");
            this.Cards.Initialize();
            this.Lanes.Initialize();
        }
    }
}