using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Users.Entities.Models
{
    public class User : IdentityUser
    {
        public User() : base()
        {
        }

        public string Name { set; get; }

        public string Surname { set; get; }

        public int PostsCount { get; set; }

        public int AnswersCount { get; set; }
    }
}
