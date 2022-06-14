namespace DewIt.Client.model.enumerations
{
    public interface IUser : IUnique, IHaveDisplayName, IHaveIcon
    {
        string UserName { get; set; }

        string Password { get; set; }
    }
}