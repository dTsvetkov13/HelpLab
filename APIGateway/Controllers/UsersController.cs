using APIGateway.Models.InputModels;
using APIGateway.Services;
using Microservices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RabbitMqService _rabbitMqService;
        public UsersController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            //TODO: Change IdentityUser with User

            string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var result = _rabbitMqService.SendWithResult<IdentityUser, string>(id, "users.get");

            return result != null ? Ok(result) : BadRequest("Invalid input");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel input)
        {
            RegisterUser registerUser = new RegisterUser
            {
                Name = input.Name,
                Surname = input.Surname,
                Email = input.Email,
                Password = input.Password
            };

            RegisterResponse result = _rabbitMqService.SendWithResult<RegisterResponse, RegisterUser>(registerUser, "users.register");

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        //Denis1
        //Uw9e?123

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputModel input)
        {
            LoginUser loginUser = new LoginUser { Username = input.UserName, Password = input.Password };

            LoginResponse result = _rabbitMqService.SendWithResult<LoginResponse, LoginUser>(loginUser, "users.login");

            if (!result.IsError)
            {
                return Ok(new
                {
                    result.AccessToken,
                    result.ExpiresIn,
                    result.TokenType
                });
            }
            else
            {
                var errors = new Dictionary<string, string[]>();

                if (result.ErrorDescription != null)
                {
                    errors["ErrDesc"] = new string[] { result.ErrorDescription };
                }
                else
                {
                    errors["Error"] = new string[] { result.Error };
                }

                return BadRequest(new { errors });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserInputModel input)
        {
            UpdateUser updateUser = new UpdateUser
            {
                CurrentEmail = input.CurrentEmail,
                CurrentPassword = input.CurrentPassword,
                CurrentUserName = input.CurrentUserName,
                NewEmail = input.NewEmail,
                NewPassword = input.NewPassword,
                NewUserName = input.NewUserName
            };

            var result = _rabbitMqService.SendWithResult<List<UpdateResponse>, UpdateUser>(updateUser, "users.update");

            foreach (var identityResult in result)
                if (!identityResult.Succeeded)
                    return BadRequest(result);
            
            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var result = _rabbitMqService.SendWithResult<DeleteResponse, string>(id, "users.delete");

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
