using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class UserClient : IUserClient
    {
        private readonly API _api;

        public UserClient(API api)
        {
            _api = api;
        }

        public IEnumerable<User> All => _api.Get().GetAll<User>(User.Url);

        public User this[int id] => _api.Get().To<User>(User.Url + "/" + id);

        public User Create(UserUpsert user) => _api.Post().With(user).To<User>(User.Url);

        public User Update(int id, UserUpsert user) => _api.Put().With(user).To<User>(User.Url + "/" + id);

        public Session Current => _api.Get().To<Session>("/user");

        public void Delete(int userId)
        {
            _api.Delete().To<User>(User.Url + "/" + userId);
        }

    }
}