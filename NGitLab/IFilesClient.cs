using NGitLab.Models;

namespace NGitLab
{
    public interface IFilesClient
    {
        void Create(FileUpsert file);

        void Update(FileUpsert file);

        void Delete(FileDelete file);

        FileData Get(string filePath, string @ref);

        bool FileExists(string filePath, string @ref);

        Blame[] Blame(string filePath, string @ref);
    }
}