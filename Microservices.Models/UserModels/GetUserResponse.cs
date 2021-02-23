using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models.UserModels
{
    public class GetUserResponse : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public int PostsCount { get; set; }

        public int AnswersCount { get; set; }
    }
}
