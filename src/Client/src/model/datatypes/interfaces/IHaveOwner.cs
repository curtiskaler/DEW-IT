namespace DewIt.Client.model.enumerations
{
    public interface IHaveOwner
    {
        IUser Owner { get; set; }
    }
}