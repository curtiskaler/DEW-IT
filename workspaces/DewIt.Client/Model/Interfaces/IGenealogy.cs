using B7.Identifiers;

namespace DewIt.Client.Model;

public interface IGenealogy
{
    Urn Project { get; set; }
    Urn Board { get; set; }
    Urn Group { get; set; }
    Urn Lane { get; set; }
    Urn Card { get; set; }
}
