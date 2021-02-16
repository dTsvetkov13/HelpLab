using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
