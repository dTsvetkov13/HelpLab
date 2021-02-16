using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Answers.Models.InputModels
{
    public class CreateInputModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime PublishedAt { get; set; }

        [Required]
        public string AuthorId { get; set; }
    }
}
