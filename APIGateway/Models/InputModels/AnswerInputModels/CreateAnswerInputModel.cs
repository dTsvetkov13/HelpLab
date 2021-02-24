using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models.InputModels
{
    public class CreateAnswerInputModel
    {
        [Required]
        public string Text { get; set; }

        public string PostId { get; set; }

        public string AnswerId { get; set; }
    }
}