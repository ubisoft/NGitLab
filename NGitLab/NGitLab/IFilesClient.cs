using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IFilesClient
    {
        IEnumerable<FileData> All { get; }
        void Create(FileUpsert file);
        void Update(FileUpsert file);
        void Delete(FileDelete file);
    }
}