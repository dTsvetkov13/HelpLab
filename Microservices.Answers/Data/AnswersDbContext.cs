using Microservices.Answers.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Answers.Data
{
    class AnswersDbContext : DbContext
    {
        public AnswersDbContext(DbContextOptions<AnswersDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Answer> Answers { get; set; }
    }
}