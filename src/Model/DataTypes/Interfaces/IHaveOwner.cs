namespace DewIt.Model.DataTypes
{
    public interface IHaveOwner
    {
        IUser Owner { get; set; }
    }
}