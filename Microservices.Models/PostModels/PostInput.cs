﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models.PostModels
{
    public class PostInput
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}
