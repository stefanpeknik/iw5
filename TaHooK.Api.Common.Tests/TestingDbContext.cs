﻿using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL;
using Microsoft.EntityFrameworkCore;

namespace TaHooK.Api.Common.Tests
{
    public class TestingDbContext : TaHooKDbContext
    {
        private readonly bool _seedTestingData;

        public TestingDbContext(DbContextOptions<TaHooKDbContext> options, bool seedTestingData = false)
            : base(options, seedDemoData: false)
        {
            _seedTestingData = seedTestingData;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (_seedTestingData)
            {
                QuizSeeds.Seed(modelBuilder);
                QuestionSeeds.Seed(modelBuilder);
                AnswerSeeds.Seed(modelBuilder);
                ScoreSeeds.Seed(modelBuilder);
                UserSeeds.Seed(modelBuilder);
            }
        }
    }
}