﻿using Microservices.Posts.Entities;
using Microservices.Posts.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Posts.Services
{
    public class PostService
    {
        private readonly PostsDbContext _dbContext;

        public PostService(PostsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EntityEntry> Create(string title, string description,
                      DateTime publishedAt, string authorId)
        {
            var result = await _dbContext.AddAsync(new Post { Title = title, Description = description,
                                                              PublishedAt = publishedAt, AuthorId = authorId
                                                            });
            
            _dbContext.SaveChanges();

            return result;
        }

        public async Task<Post> Get(string id)
        {
            Post result = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            return result;
        }

        public async Task<EntityEntry> Delete(string id)
        {
            EntityEntry result = _dbContext.Posts.Remove(await Get(id));
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}