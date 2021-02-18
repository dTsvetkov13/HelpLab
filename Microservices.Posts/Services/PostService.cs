using Microservices.Posts.Entities;
using Microservices.Posts.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Models.Messages;
using Microservices.EventBus;
using Microservices.EventBus.Interfaces;

namespace Microservices.Posts.Services
{
    public class PostService
    {
        private readonly PostsDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public PostService(PostsDbContext dbContext,
                           IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public async Task<EntityEntry> Create(string title, string description,
                      DateTime publishedAt, string authorId)
        {
            var result = await _dbContext.AddAsync(new Post { Title = title, Description = description,
                                                              PublishedAt = publishedAt, AuthorId = authorId
                                                            });
            
            _dbContext.SaveChanges();

            //Send message to the Microservices.Users
            _eventBus.Publish(authorId, MessagesEnum.PostsCreatedRoute);

            return result;
        }

        public async Task<Post> Get(string id)
        {
            Post result = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            return result;
        }

        public async Task<EntityEntry> Delete(string id)
        {
            var currentPost = await Get(id);
            EntityEntry result = _dbContext.Posts.Remove(currentPost);
            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(currentPost.AuthorId, MessagesEnum.PostsDeletedRoute);

            return result;
        }
    }
}
