using IdentityModel.Client;
using Microservices.Users.Entities.Models;
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
        Task<IdentityResult> Create(string email, string password, string name, string surname);

        Task<IEnumerable<IdentityResult>> Update(
            string currentUserName,
            string newUserName,
            string currentPassword,
            string newPassword,
            string currentEmail,
            string newEmail
        );

        Task<User> Get(string id);
        Task<IdentityResult> Delete(string id);

        Task IncreasePostsCount(string id);
    }
}
