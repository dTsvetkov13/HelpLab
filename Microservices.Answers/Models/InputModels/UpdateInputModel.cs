using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Answers.Models.InputModels
{
    public class UpdateInputModel
    {
        [Required]
        public string AnswerId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
