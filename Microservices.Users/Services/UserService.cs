using Microservices.Users;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microservices.Users.Entities;
using Microservices.Users.Entities.Models;

namespace Microservices.Users
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<TokenResponse> Authenticate(string username, string password)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            TokenResponse result = await new HttpClient(httpClientHandler).RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                //Move this into variable          
                Address = "https://localhost:44337/connect/token",

                ClientId = "api",
                Scope = "Microservices.Users",

                AuthorizationHeaderStyle = BasicAuthenticationHeaderStyle.Rfc2617,

                UserName = username,
                Password = password
            });
            Console.WriteLine("Is error: " + result.IsError + " " + result.Error + " " + result.ErrorDescription);
            Console.WriteLine("Token: " + result.AccessToken);

            return result;
        }

        public async Task<IdentityResult> Create(string email, string password, string name, string surname)
        {
            IdentityResult result = await _userManager.CreateAsync(new User { Name = name, Surname = surname, 
                                                                   UserName = email, Email = email }, password);

            return result;
        }

        public async Task<IEnumerable<IdentityResult>> Update(
            string currentUserName,
            string newUserName,
            string currentPassword,
            string newPassword,
            string currentEmail,
            string newEmail
        )
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == currentUserName);
            var result = new List<IdentityResult>();
            if (user == null)
                return new[]
                {
                    IdentityResult.Failed(new IdentityError
                    {
                        Code = "UserNotFound", Description = "User could not be found"
                    })
                };
            if (newUserName != null) result.Add(await _userManager.SetUserNameAsync(user, newUserName));
            if (newPassword != null) result.Add(await _userManager.ChangePasswordAsync(user, currentPassword, newPassword));
            if (newEmail != null) result.Add(await _userManager.ChangeEmailAsync(user, newEmail, await _userManager.GenerateChangeEmailTokenAsync(user, newEmail)));

            return result;
        }

        public async Task<User> Get(string id)
        {
            User result = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<IdentityResult> Delete(string id)
        {
            IdentityResult result = await _userManager.DeleteAsync(await Get(id));

            return result;
        }
    }
}
