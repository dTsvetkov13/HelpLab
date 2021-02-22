using APIGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private static readonly string AnswersRoot = "https://localhost:44367/api/microservices/answers";
        private readonly HttpSender _httpSender;

        public AnswersController(HttpSender httpSender)
        {
            _httpSender = httpSender;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _httpSender.SendGetAsync<GetPostResponse>(AnswersRoot + "?id=" + id);

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

            var user = await _httpSender.SendGetAsync<GetUserResponse>(AnswersRoot + "?id=" + userId);

            PostInput post = new PostInput
            {
                Title = input.Title,
                Description = input.Description,
                PublishedAt = DateTime.UtcNow,
                AuthorId = userId,
                AuthorName = user.Name
            };

            SimpleRequestResponse result;

            try
            {
                result = await _httpSender.SendPostAsync<SimpleRequestResponse, PostInput>(post, AnswersRoot);
            }
            catch (Exception)
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
                response = await _httpSender.SendDeleteAsync<SimpleRequestResponse>(AnswersRoot + "?id=" + id + "&userId=" + userId);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            if (response.Succeeded)
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
