using Microservices.Users.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Users.Controllers
{
    [Route("api/microservices/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public void Get()
        {

        }

        [HttpPost]
        public void Create(RegisterInputModel input)
        {

        }

        [HttpPost("login")]
        public void Login(LoginInputModel input)
        {

        }

        [HttpPut]
        public void Update(UpdateUserInputModel input)
        {

        }

        [HttpDelete]
        public void Delete()
        {

        }
    }
}
