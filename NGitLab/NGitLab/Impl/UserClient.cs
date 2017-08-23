using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class UserClient : IUserClient {
        readonly Api api;

        public UserClient(Api api) {
            this.api = api;
        }

        public IEnumerable<User> All() {
            return api.Get().GetAll<User>(User.Url);
        }

        public User Get(int id) {
            return api.Get().To<User>(User.Url + "/" + id);
        }

        public User Create(UserUpsert user) {
            return api.Post().With(user).To<User>(User.Url);
        }

        public User Update(int id, UserUpsert user) {
            return api.Put().With(user).To<User>(User.Url + "/" + id);
        }

        public void Delete(int userId) {
            api.Delete().To<User>(User.Url + "/" + userId);
        }

        public Session Current() {
            return api.Get().To<Session>("/user");
        }
    }
}