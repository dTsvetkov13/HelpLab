using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models.PostModels
{
    public class GetPostResponse
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime LastEditedAt { get; set; }

        public string AuthorName { get; set; }
    }
}
