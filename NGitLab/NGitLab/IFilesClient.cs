using NGitLab.Models;

namespace NGitLab {
    public interface IFilesClient {
        void Create(FileUpsert file);
        void Update(FileUpsert file);
        void Delete(FileDelete file);
    }
}