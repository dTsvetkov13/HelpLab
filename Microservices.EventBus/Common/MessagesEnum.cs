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
    }
}
