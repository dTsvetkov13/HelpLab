using APIGateway.Models.InputModels;
using APIGateway.Services;
using Microservices.Models;
using Microservices.Models.PostModels;
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
        private readonly RabbitMqService _rabbitMqService;
        private readonly HttpSender _httpSender;

        public PostsController(RabbitMqService rabbitMqService, HttpSender httpSender)
        {
            _rabbitMqService = rabbitMqService;
            _httpSender = httpSender;
        }

        [HttpGet]
        public IActionResult Get(string id)
        {
            var result = _rabbitMqService.SendWithResult<GetPostResponse, string>(id, "posts.get"); ;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("There is no such post.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostInputModel input)
        {
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            PostInput post = new PostInput
            {
                Title = input.Title,
                Description = input.Description,
                PublishedAt = DateTime.UtcNow,
                AuthorId = userId
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
        public void Update(string s)
        {

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
