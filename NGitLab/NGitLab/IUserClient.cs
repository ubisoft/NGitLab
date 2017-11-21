using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IUserClient
    {
        IEnumerable<User> All { get; }
        User this[int id] { get; }
        User Create(UserUpsert user);
        User Update(int id, UserUpsert user);

        /// <summary>
        /// Request a token for user impersonation.
        /// Admin account/token is required for impersonation
        /// </summary>
        /// <param name="tokenRequest">info required to create the token</param>
        /// <returns></returns>
        UserToken CreateToken(UserTokenCreate tokenRequest);
        void Delete(int id);
        Session Current { get; }

        /// <summary>
        /// Allows to manipulate the ssh keys of the currently logged user.
        /// Does not need admin rights.
        /// </summary>
        ISshKeyClient CurrentUserSShKeys { get; }

        /// <summary>
        /// Allows to manipulate the ssh keys of a given user, needs gitlab admin rights.
        /// </summary>
        ISshKeyClient SShKeys(int userId);
    }
}