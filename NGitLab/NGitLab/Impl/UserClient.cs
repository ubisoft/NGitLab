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

        public void Add(User user)
        {
            throw new System.NotImplementedException();
        }

        public void Update(User user)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new System.NotImplementedException();
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