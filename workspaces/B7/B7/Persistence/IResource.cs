using System;
namespace B7.Persistence;

public interface IResource
{
    string GetPersistencePath();
    void DeleteResource(string path);
}

