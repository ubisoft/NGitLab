using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IUserClient {
        IEnumerable<User> All();
        User Get(int id);
        User Create(UserUpsert user);
        User Update(int id, UserUpsert user);
        void Delete(int id);

        Session Current();
    }
}