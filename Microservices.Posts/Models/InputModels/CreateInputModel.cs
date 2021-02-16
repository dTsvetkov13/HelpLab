using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Posts.Models.InputModels
{
    public class CreateInputModel
    {
        //TODO: think of a appropriate annotation
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime PublishedAt { get; set; }

        [Required]
        public string AuthorId { get; set; }
    }
}
