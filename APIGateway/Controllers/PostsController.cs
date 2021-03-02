using APIGateway.Models.InputModels;
using APIGateway.Models.InputModels.PostInputModels;
using APIGateway.Services;
using Microservices.Models;
using Microservices.Models.Common;
using Microservices.Models.PostModels;
using Microservices.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class PostsController : ControllerBase
    {
        private static readonly string PostsRoot = "https://localhost:44367/api/microservices/posts";
        private readonly HttpSender _httpSender;

        public PostsController(RabbitMqService rabbitMqService, HttpSender httpSender)
        {
            _httpSender = httpSender;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _httpSender.SendGetAsync<GetPostResponse>(PostsRoot + "/" + id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("There is no such post.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPostInputModel input)
        {
            var result = await _httpSender.SendGetAsync<List<GetPostResponse>>(PostsRoot + "?UserId=" + input.UserId);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreatePostInputModel input)
        {
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _httpSender.SendGetAsync<GetUserResponse>(UsersController.UsersRoot  + "?id=" + userId);

            PostInput post = new PostInput
            {
                Title = input.Title,
                Description = input.Description,
                AuthorId = userId,
                AuthorName = user.Name
            };

            SimpleRequestResponse result;

            try
            {
                result = await _httpSender.SendPostAsync<SimpleRequestResponse, PostInput>(post, PostsRoot);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, result.Error);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdatePostInputModel input)
        {
            var result = await _httpSender.SendPutAsync<Response, UpdatePostInputModel>(input, PostsRoot);

            if (result.Status == Status.Ok)
            {
                return Ok(new Response
                {
                    Status = result.Status,
                    Error = result.Error
                });
            }
            else if (result.Status == Status.InvalidData)
            {
                return BadRequest(new Response
                {
                    Status = result.Status,
                    Error = result.Error
                });
            }
            else
            {
                return StatusCode(500, (new Response
                {
                    Status = result.Status,
                    Error = result.Error
                }));
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            SimpleRequestResponse response = new SimpleRequestResponse();

            try
            {
                response = await _httpSender.SendDeleteAsync<SimpleRequestResponse>(PostsRoot + "?id=" + id + "&userId=" + userId);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
            if(response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
