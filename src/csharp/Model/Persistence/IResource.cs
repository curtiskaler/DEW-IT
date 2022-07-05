namespace DewIt.Model.Persistence;

public interface IResource
{
    string GetPath(string filename = "DewIt.db");

    void DeleteResource(string path);
}
