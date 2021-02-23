using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models.AnswerModels
{
    public class GetAnswerModel
    {
        public string Text { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime LastEditedAt { get; set; }

        public string AuthorId { get; set; }

        //TODO: add attribute
        public Guid PostId { get; set; }

        public Guid AnswerId { get; set; }
    }
}
