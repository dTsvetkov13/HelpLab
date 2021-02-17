using IdentityModel.Client;
using Microservices.Models;
using Microservices.Users.Entities.Models;
using Microservices.Users.Models.InputModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Users.Controllers
{
    [Route("api/microservices/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            User result = await _userService.Get(id);

            if(result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterInputModel input)
        {
            IdentityResult  result = await _userService.Create(input.Email, input.Password, 
                                                               input.Name, input.Surname);

            RegisterResponse registerResponse = new RegisterResponse
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors
            };

            if(registerResponse.Succeeded)
            {
                return Ok(registerResponse);
            }
            else
            {
                return BadRequest(registerResponse);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputModel input)
        {
            TokenResponse result = await _userService.Authenticate(input.Email, input.Password);

            if(!result.IsError)
            {
                return Ok(new LoginResponse
                { 
                    IsError = result.IsError,
                    AccessToken = result.AccessToken,
                    TokenType = result.TokenType
                });
            }
            else
            {
                return BadRequest(new LoginResponse
                {
                    IsError = result.IsError,
                    Error = result.Error,
                    ErrorDescription = result.ErrorDescription
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserInputModel input)
        {
            IEnumerable<IdentityResult> result = new List<IdentityResult>();

            result = await _userService.Update(input.CurrentUserName, input.NewUserName,
                                               input.CurrentPassword, input.CurrentPassword,
                                               input.CurrentEmail, input.NewEmail);

            foreach (var identityResult in result)
                if (!identityResult.Succeeded)
                    return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityResult result = await _userService.Delete(id);

            DeleteResponse deleteResponse = new DeleteResponse
            {
                Errors = result.Errors,
                Succeeded = result.Succeeded
            };

            if (deleteResponse.Succeeded)
            {
                return Ok(deleteResponse);
            }
            else
            {
                return BadRequest(deleteResponse);
            }
        }
    }
}
