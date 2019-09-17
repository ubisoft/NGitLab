using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class UserClient : ClientBase, IUserClient
    {
        public UserClient(ClientContext context)
            : base(context)
        {
        }

        public Models.User this[int id] => Server.Users.GetById(id)?.ToClientUser() ?? throw new GitLabNotFoundException();

        public IEnumerable<Models.User> All => Server.Users.Select(user => user.ToClientUser());

        public Session Current => Context.User?.ToClientSession();

        public ISshKeyClient CurrentUserSShKeys => throw new NotSupportedException();

        public Models.User Create(UserUpsert user)
        {
            var u = new User(user.Username)
            {
                Name = user.Name,
                Email = user.Email,
            };

            Server.Users.Add(u);
            return u.ToClientUser();
        }

        public UserToken CreateToken(UserTokenCreate tokenRequest)
        {
            throw new NotSupportedException();
        }

        public void Delete(int id)
        {
            var user = Server.Users.GetById(id);
            if (user != null)
            {
                Server.Users.Remove(user);
            }
            else
            {
                throw new GitLabNotFoundException();
            }
        }

        public IEnumerable<Models.User> Get(string username)
        {
            return Server.Users.SearchByUsername(username).Select(user => user.ToClientUser());
        }

        public IEnumerable<Models.User> Search(string query)
        {
            return Server.Users.SearchByUsername(query).Select(user => user.ToClientUser());
        }

        public ISshKeyClient SShKeys(int userId)
        {
            throw new NotImplementedException();
        }

        public Models.User Update(int id, UserUpsert userUpsert)
        {
            var user = Server.Users.GetById(id);
            if (user != null)
            {
                user.Name = userUpsert.Name;
                user.Email = userUpsert.Email;

                return user.ToClientUser();
            }
            else
            {
                throw new GitLabNotFoundException();
            }
        }
    }
}
