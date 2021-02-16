using Microservices.Posts.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Posts.Controllers
{
    [Route("api/microservices/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        public void Get()
        {

        }

        [HttpPost]
        public void Create(CreateInputModel input)
        {

        }

        [HttpPut]
        public void Update(UpdateInputModel input)
        {

        }

        [HttpDelete]
        public void Delete()
        {

        }
    }
}
