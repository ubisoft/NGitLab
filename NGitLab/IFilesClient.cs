using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface IFilesClient
    {
        void Create(FileUpsert file);

        void Update(FileUpsert file);

        void Delete(FileDelete file);

        FileData Get(string filePath, string @ref);

        Task<FileData> GetAsync(string filePath, string @ref, CancellationToken cancellationToken = default);

        bool FileExists(string filePath, string @ref);

        Blame[] Blame(string filePath, string @ref);
    }
}
