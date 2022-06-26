namespace DewIt.Model.DataTypes
{
    public interface IGenealogy
    {
        URN Project { get; set; }
        URN Board { get; set; }
        URN Group { get; set; }
        URN Lane { get; set; }
        URN Card { get; set; }
    }
}