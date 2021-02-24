using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models.InputModels.AnswerInputModels
{
    public class UpdateAnswerInputModel
    {
        [Required]
        public string AnswerId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
