using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Users
{
    public interface IUserService
    {
        Task<TokenResponse> Authenticate(string username, string password);
        Task<IdentityResult> Create(string username, string password, string email);

        Task<IEnumerable<IdentityResult>> Update(
            string currentUserName,
            string newUserName,
            string currentPassword,
            string newPassword,
            string currentEmail,
            string newEmail
        );

        Task<IdentityUser> Get(string id);
        Task<IdentityResult> Delete(string id);
    }
}
