using Microservices.Models.Common;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Post post = await _postService.Get(id);

            if (post != null)
            {
                return Ok(post);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInputModel input)
        {
            Status result = await _postService.Create(input.Title, input.Description,
                                                      DateTime.UtcNow, input.AuthorId,
                                                      input.AuthorName);
                
            if (result == Status.Ok)
            {
                return Ok(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else if(result == Status.InvalidData)
            {
                return BadRequest(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else
            {
                return StatusCode(500, (new Response
                {
                    Status = result,
                    Error = ""
                }));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateInputModel input)
        {
            Guid guidPostId = input.PostId != null ? Guid.Parse(input.PostId) : Guid.Empty;

            var result = await _postService.Update(guidPostId, input.Title,
                                                   input.Description, DateTime.UtcNow);

            if (result == Status.Ok)
            {
                return Ok(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else if (result == Status.InvalidData)
            {
                return BadRequest(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else
            {
                return StatusCode(500, (new Response
                {
                    Status = result,
                    Error = ""
                }));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id, string userId)
        {
            Post post = await _postService.Get(id);
            

            if(post.AuthorId == userId)
            {
                var result = await _postService.Delete(id);

                if (result == Status.Ok)
                {
                    return Ok(new Response
                    {
                        Status = result,
                        Error = ""
                    });
                }
                else if (result == Status.InvalidData)
                {
                    return BadRequest(new Response
                    {
                        Status = result,
                        Error = ""
                    });
                }
                else
                {
                    return StatusCode(500, (new Response
                    {
                        Status = result,
                        Error = ""
                    }));
                }
            }
            else
            {
                return BadRequest(new Response
                {
                    Status = Status.InvalidData,
                    Error = "You are not the author, thus you cannot delete this post"
                });
            }
        }
    }
}
