using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Posts.Entities.Models
{
    public class User
    {
        [Required]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}