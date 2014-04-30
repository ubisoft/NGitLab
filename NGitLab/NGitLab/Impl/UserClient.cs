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

        public IEnumerable<User> All
        {
            get
            {
                return _api.Retrieve().GetAll<User>(User.Url);
            }
        }

        public User this[int id]
        {
            get
            {
                return _api.Retrieve().To<User>(User.Url + "/" + id);
            }
        }

        public void Add(UserUpsert user)
        {
            _api.Retrieve().Method(MethodType.Post).With(user).To<User>(User.Url);
        }

        public void Update(int id, UserUpsert user)
        {
            _api.Retrieve().Method(MethodType.Put).With(user).To<User>(User.Url + "/" + id);
        }

        public void Delete(User user)
        {
            _api.Retrieve().Method(MethodType.Delete).To<User>(User.Url + "/" + user.Id);
        }

        public Session Current
        {
            get
            {
                return _api.Retrieve().To<Session>("/user");
            }
        }
    }
}