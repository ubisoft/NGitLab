using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IUserClient
    {
        IEnumerable<User> All { get; }
        User this[int id] { get; }
        void Add(User user);
        void Update(User user);
        void Delete(User user);

        Session Current { get; }
    }
}