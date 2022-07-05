using DewIt.Model.Persistence;

namespace DewIt.Client;

public class DBResource : IResource
{
    public string GetPath(string filename)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);
    }

    public void DeleteResource(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}