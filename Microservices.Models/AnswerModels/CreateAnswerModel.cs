using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microservices.Models.AnswerModels
{
    public class CreateAnswerModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string PostId { get; set; }

        public string AnswerId { get; set; }
    }
}
