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
using Microservices.Models.Common;

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

        public async Task<Status> Create(string title, string description,
                      DateTime publishedAt, string authorId, string authorName)
        {
            try
            {
                var result = await _dbContext.AddAsync(new Post
                {
                    Title = title,
                    Description = description,
                    PublishedAt = publishedAt,
                    AuthorId = authorId
                });

                if(result.State == EntityState.Added)
                {
                    if ((await _dbContext.Users.FirstOrDefaultAsync(User => User.Id == authorId)) == null)
                    {
                        await _dbContext.AddAsync(new User
                        {
                            Id = authorId,
                            Name = authorName
                        });
                    }

                    _dbContext.SaveChanges();

                    //Publish message to the Event bus
                    _eventBus.Publish(authorId, MessagesEnum.PostsCreatedRoute);
                }
                else
                {
                    return Status.InvalidData;
                }
            }
            catch(Exception e)
            {
                return Status.ServerError;
            }

            return Status.Ok;
        }

        public async Task<Post> Get(string id)
        {
            Post result = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            return result;
        }

        public async Task<Status> Delete(string id)
        {
            try
            {
                var currentPost = await Get(id);
                EntityEntry result = _dbContext.Posts.Remove(currentPost);

                await _dbContext.SaveChangesAsync();

                _eventBus.Publish(currentPost.AuthorId, MessagesEnum.PostsDeletedRoute);
            }
            catch(Exception e)
            {
                return Status.ServerError;
            }

            return Status.Ok;
        }

        public async Task<Status> Update(Guid postId, string title, string description, DateTime editedAt)
        {
            try
            {
                if(postId == Guid.Empty)
                {
                    return Status.InvalidData;
                }

                var post = await _dbContext.Posts.FirstOrDefaultAsync<Post>(post => post.Id == postId);

                if (post == null)
                {
                    return Status.InvalidData;
                }

                if (title != null)
                {
                    post.Title = title;
                    post.LastEditedAt = editedAt;
                }

                if (description != null)
                {
                    post.Description = description;
                    post.LastEditedAt = editedAt;
                }

                await _dbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return Status.ServerError;
            }

            return Status.Ok;
        }

        public void IncreaseAnswersCount(string postId)
        {
            Get(postId).Result.AnswersCount++;

            _dbContext.SaveChangesAsync();
        }

        public void DecreaseAnswersCount(string postId)
        {
            Get(postId).Result.AnswersCount--;

            _dbContext.SaveChangesAsync();
        }
    }
}
