using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.EventBus
{
    public class MessagesEnum
    {
        public const string PostsRoute = "posts";
        public const string PostsCreatedRoute = PostsRoute + ".created";
        public const string PostsDeletedRoute = PostsRoute + ".deleted";

        public const string AnswersRoute = "answers";
        public const string AnswersCreatedRoute = AnswersRoute + ".created";
        public const string AnswersDeletedRoute = AnswersRoute + ".deleted";
    }
}
