﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models.InputModels
{
    public class UpdatePostInputModel
    {
        [Required]
        public string PostId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
