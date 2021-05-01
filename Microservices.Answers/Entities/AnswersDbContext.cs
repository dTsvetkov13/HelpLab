using Microservices.Answers.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Answers.Entities
{
    public class AnswersDbContext : DbContext
    {
        public AnswersDbContext(DbContextOptions<AnswersDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<User> Users { get; set; }
    }
}