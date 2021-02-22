using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models.AnswerModels
{
    public class CreateAnswerModel
    {
        public string Text { get; set; }

        public DateTime PublishedAt { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string PostId { get; set; }

        public string AnswerId { get; set; }
    }
}
