namespace DewIt.Model.DataTypes
{
    public interface IUser : IUnique, IHaveDisplayName, IHaveIcon
    {
        string UserName { get; set; }

        string Password { get; set; }
    }
}