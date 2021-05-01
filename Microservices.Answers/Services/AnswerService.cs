using Microservices.Answers.Entities;
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

        public async Task<Status> Create(string text, DateTime publishedAt, 
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
                    return Status.InvalidData;
                }

                /*if ((await _dbContext.Users.FirstOrDefaultAsync(User => User.Id == authorId)) == null)
            {
                await _dbContext.AddAsync(new User
                {
                    Id = authorId,
                    Name = authorName
                });
            }*/
                if(guidAnswerId != Guid.Empty
                   && guidPostId == Guid.Empty)
                {
                    Get(answerId).Result.AnswersCount++;
                }

                _dbContext.SaveChanges();

                //Send message to the Microservices.Users
                _eventBus.Publish(authorId, MessagesEnum.AnswersCreatedRoute);
            }
            catch (Exception e)
            {
                return Status.ServerError;
            }

            return Status.Ok;
        }

        public async Task<Answer> Get(string id)
        {
            Answer result = await _dbContext.Answers.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            return result;
        }

        public List<Answer> GetByPostId(string id)
        {
            var result = _dbContext.Answers.Where(x => x.PostId.ToString() == id).ToList();
            return result;
        }

        public async Task<Status> Update(Guid answerId, string text, DateTime editedAt)
        {
            try
            {
                if (answerId == Guid.Empty)
                {
                    return Status.InvalidData;
                }

                var answer = await _dbContext.Answers.FirstOrDefaultAsync<Answer>(answer => answer.Id == answerId);

                if (answer == null)
                {
                    return Status.InvalidData;
                }

                if (text != null)
                {
                    answer.Text = text;
                    answer.LastEditedAt = editedAt;
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return Status.ServerError;
            }

            return Status.Ok;
        }

        public async Task<Status> Delete(string id)
        {
            try
            {
                Answer currentAnswer = await Get(id);

                if(currentAnswer == null)
                {
                    return Status.InvalidData;
                }

                EntityEntry result = _dbContext.Answers.Remove(currentAnswer);
                await _dbContext.SaveChangesAsync();
                
                _eventBus.Publish(currentAnswer.AuthorId, MessagesEnum.AnswersDeletedRoute);
            }
            catch(Exception)
            {
                return Status.ServerError;
            }

            return Status.Ok;
        }
    }
}
