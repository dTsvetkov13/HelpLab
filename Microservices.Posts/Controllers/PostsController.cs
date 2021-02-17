using Microservices.Models.PostModels;
using Microservices.Posts.Entities.Models;
using Microservices.Posts.Models.InputModels;
using Microservices.Posts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservices.Posts.Controllers
{
    [Route("api/microservices/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;
        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public void Get()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInputModel input)
        {
            EntityEntry result;
            SimpleRequestResponse postResponse = new SimpleRequestResponse();

            try
            {
                result = await _postService.Create(input.Title, input.Description,
                                                  input.PublishedAt, input.AuthorId);

                if (result.State == Microsoft.EntityFrameworkCore.EntityState.Unchanged)
                {
                    postResponse = new SimpleRequestResponse
                    {
                        Error = "",
                        IsError = false,
                        Succeeded = true
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(" [.] " + e.Message);

                //TODO: log e.Message

                postResponse = new SimpleRequestResponse
                {
                    Error = "This post cannot be added now. Excuse us!",
                    IsError = true,
                    Succeeded = false
                };
            }

            if(postResponse.Succeeded)
            {
                return Ok(new SimpleRequestResponse
                {
                    Succeeded = true,
                    Error = "",
                    IsError = false
                });
            }
            else
            {
                return BadRequest(postResponse);
            }
        }

        [HttpPut]
        public void Update(UpdateInputModel input)
        {

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id, string userId)
        {
            Post post = await _postService.Get(id);
            SimpleRequestResponse response;

            if(post.AuthorId == userId)
            {
                try
                {
                    var result = await _postService.Delete(id);

                    response = new SimpleRequestResponse
                    {
                        Error = "",
                        IsError = false,
                        Succeeded = true
                    };

                    return Ok(response);
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }
            else
            {
                response = new SimpleRequestResponse
                {
                    Error = "You cannot delete this post!",
                    IsError = true,
                    Succeeded = false
                };

                return BadRequest(response);
            }
        }
    }
}
