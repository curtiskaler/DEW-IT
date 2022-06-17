namespace DewIt.Client.model.enumerations
{
    public interface IGenealogy
    {
        IProject Project { get; set; }
        IBoard Board { get; set; }
        IGroup Group { get; set; }
        ILane Lane { get; set; }
        ICard Card { get; set; }
    }
}