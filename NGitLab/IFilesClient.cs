using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IFilesClient
{
    void Create(FileUpsert file);

    Task UpdateAsync(FileUpsert file, CancellationToken cancellationToken = default);

    void Update(FileUpsert file);

    Task CreateAsync(FileUpsert file, CancellationToken cancellationToken = default);

    void Delete(FileDelete file);

    Task DeleteAsync(FileDelete file, CancellationToken cancellationToken = default);

    FileData Get(string filePath, string @ref);

    Task<FileData> GetAsync(string filePath, string @ref, CancellationToken cancellationToken = default);

    bool FileExists(string filePath, string @ref);

    Task<bool> FileExistsAsync(string filePath, string @ref, CancellationToken cancellationToken = default);

    Blame[] Blame(string filePath, string @ref);
}
