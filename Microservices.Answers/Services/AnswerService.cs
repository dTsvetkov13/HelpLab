﻿using Microservices.Answers.Entities;
using Microservices.Answers.Entities.Models;
using Microservices.EventBus;
using Microservices.EventBus.Interfaces;
using Microservices.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Answers.Services
{
    public class AnswerService
    {
        private readonly AnswersDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public AnswerService(AnswersDbContext dbContext,
                           IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public async Task<Statuses> Create(string text, DateTime publishedAt, 
                                              string authorId, string authorName,
                                              string postId, string answerId)
        {
            try
            {
                Guid guidPostId = postId != null ? Guid.Parse(postId) : Guid.Empty;
                Guid guidAnswerId = answerId != null ? Guid.Parse(answerId) : Guid.Empty;

                EntityEntry result = await _dbContext.AddAsync(new Answer
                {
                    Text = text,
                    PublishedAt = publishedAt,
                    AuthorId = authorId,
                    AnswerId = guidAnswerId,
                    PostId = guidPostId,
                });

                if(result.State != EntityState.Added)
                {
                    return Statuses.InvalidData;
                }

                /*if ((await _dbContext.Users.FirstOrDefaultAsync(User => User.Id == authorId)) == null)
            {
                await _dbContext.AddAsync(new User
                {
                    Id = authorId,
                    Name = authorName
                });
            }*/
                _dbContext.SaveChanges();

                //Send message to the Microservices.Users
                _eventBus.Publish(authorId, MessagesEnum.AnswersCreatedRoute);
            }
            catch (Exception e)
            {
                return Statuses.ServerError;
            }

            return Statuses.Ok;
        }

        public async Task<Answer> Get(string id)
        {
            Answer result = await _dbContext.Answers.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            return result;
        }

        public async Task<Statuses> Delete(string id)
        {
            try
            {
                Answer currentAnswer = await Get(id);

                if(currentAnswer == null)
                {
                    return Statuses.InvalidData;
                }

                EntityEntry result = _dbContext.Answers.Remove(currentAnswer);
                await _dbContext.SaveChangesAsync();
                
                _eventBus.Publish(currentAnswer.AuthorId, MessagesEnum.AnswersDeletedRoute);
            }
            catch(Exception)
            {
                return Statuses.ServerError;
            }

            return Statuses.Ok;
        }
    }
}