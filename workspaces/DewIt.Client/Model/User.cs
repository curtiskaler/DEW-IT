using B7.Identifiers;

namespace DewIt.Client.Model;

public interface IUser : IUniqueObject, IHaveDisplayName, IHaveIcon
{
    string UserName { get; set; }

    // FIXME: NO NO NO NOT A STRING
    string Password { get; set; }

    public static readonly IUser UNSPECIFIED = new User
    {
        ID = Guid.Empty,
        DisplayName = "unspecified",
        UserName = "unspecified",
        Password = string.Empty
    };
}

public class User : IUser
{
    public Guid ID { get; init; } = Guid.NewGuid();
    public string DisplayName { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}
