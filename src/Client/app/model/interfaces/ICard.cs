namespace DewIt.Client.model.enumerations
{
    public interface ICard : IUnique, IHaveDisplayName, IHaveDescription, IHaveOrder, IHaveOwner, IHaveHistory, IHaveGenealogy
    {
        IPriority Priority { get; set; }

        ICategory Category { get; set; }

        ILabel[] Labels { get; set; }

        IComment[] Comments { get; set; }

        int Version { get; }

        void Move(IURI target);
    }
}