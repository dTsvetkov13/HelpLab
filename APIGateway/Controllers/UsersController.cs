using APIGateway.Models.InputModels;
using APIGateway.Services;
using Microservices.Models;
using Microservices.Models.UserModels;
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
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public static readonly string UsersRoot = "https://localhost:44337/api/microservices/users";
        private readonly HttpSender _httpSender;
        public UsersController(HttpSender httpSender)
        {
            _httpSender = httpSender;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            //TODO: Change IdentityUser with User

            string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            GetUserResponse result;

            try
            {
                result = await _httpSender.SendGetAsync<GetUserResponse>(UsersRoot + "?id=" + id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return result != null ? Ok(result) : BadRequest("Invalid input");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel input)
        {
            RegisterResponse result;

            try
            {
                result = await _httpSender.SendPostAsync<RegisterResponse, RegisterInputModel>(input, UsersRoot);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        //Denis1
        //Uw9e?123

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputModel input)
        {
            LoginResponse result;

            try
            {
                result = await _httpSender.SendPostAsync<LoginResponse, LoginInputModel>(input, UsersRoot + "/login");
            }
            catch(Exception)
            {
                return StatusCode(500);
            }

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
            var request = new HttpRequestMessage(HttpMethod.Put, UsersRoot);

            List<UpdateResponse> result;

            try
            {
                result = await _httpSender.SendPutAsync<List<UpdateResponse>, UpdateUserInputModel>(input, UsersRoot);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

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

            DeleteResponse result;

            try
            {
                result = await _httpSender.SendDeleteAsync<DeleteResponse>(UsersRoot + "?id=" + id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
