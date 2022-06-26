namespace DewIt.Model.DataTypes
{
    public interface IUser : IUnique, IHaveDisplayName, IHaveIcon
    {
        string UserName { get; set; }

        string Password { get; set; }

        public static readonly IUser UNSPECIFIED = new User
        {
            UUID = Guid.Empty,
            DisplayName = "unspecified",
            UserName = "unspecified",
            Password = string.Empty
        };
    }

    public class User : IUser
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}