using Microsoft.AspNetCore.Mvc;

namespace Microservices.Posts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        public void Get()
        {

        }

        [HttpPost]
        public void Create()
        {

        }

        [HttpPut]
        public void Update()
        {

        }

        [HttpDelete]
        public void Delete()
        {

        }
    }
}
