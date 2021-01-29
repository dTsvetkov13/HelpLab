using APIGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMonitoringApi.InputModels;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.Get(id);
            return result != null ? Ok(result) : BadRequest("Invalid input");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel input)
        {
            var result = await _userService.Create(input.UserName, input.Password, input.Email);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputModel input)
        {
            var result = await _userService.Authenticate(input.UserName, input.Password);
            return !result.IsError
                ? Ok(new
                {
                    result.AccessToken,
                    result.ExpiresIn,
                    result.TokenType
                })
                : BadRequest(new
                {
                    result.ErrorDescription,
                    result.Error,
                    result.ErrorType,
                    result.HttpErrorReason
                });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserInputModel input)
        {
            var result = await _userService.Update(input.CurrentUserName,
                                                   input.NewUserName,
                                                   input.CurrentPassword,
                                                   input.CurrentPassword,
                                                   input.CurrentEmail,
                                                   input.NewEmail);

            foreach (var identityResult in result)
                if (!identityResult.Succeeded)
                    return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.Delete(id);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
