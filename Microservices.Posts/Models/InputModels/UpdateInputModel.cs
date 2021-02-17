﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Posts.Models.InputModels
{
    public class UpdateInputModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime EditedAt { get; set; }
    }
}