using B7.Persistence;

namespace DewIt.Client.Persistence;

public class DBResource : IResource
{
    const string fileName = "DewIt.db";

    public void DeleteResource(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public string GetPersistencePath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
    }
}

