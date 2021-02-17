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
        private readonly IHttpClientFactory _clientFactory;
        private static readonly string UsersRoot = "https://localhost:44337/api/microservices/users";
        public UsersController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            //TODO: Change IdentityUser with User

            string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var request = new HttpRequestMessage(HttpMethod.Get, UsersRoot + "?id=" + id);
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(request);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            IdentityUser result = JsonConvert.DeserializeObject<IdentityUser>(responseString);        

            return result != null ? Ok(result) : BadRequest("Invalid input");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel input)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, UsersRoot);
            request.Content = JsonContent.Create(input);
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(request);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            RegisterResponse result = JsonConvert.DeserializeObject<RegisterResponse>(responseString);

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        //Denis1
        //Uw9e?123

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputModel input)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, UsersRoot + "/login");
            request.Content = JsonContent.Create(input);
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(request);
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            LoginResponse result = JsonConvert.DeserializeObject<LoginResponse>(responseString);

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
            request.Content = JsonContent.Create(input);
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(request);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            List<UpdateResponse> result = JsonConvert.DeserializeObject<List<UpdateResponse>>(responseString);

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
            var request = new HttpRequestMessage(HttpMethod.Delete, UsersRoot + "?id=" + id);
            
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(request);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            DeleteResponse result = JsonConvert.DeserializeObject<DeleteResponse>(responseString);

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
