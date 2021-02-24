﻿using Microservices.Posts.Entities;
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
    }
}
