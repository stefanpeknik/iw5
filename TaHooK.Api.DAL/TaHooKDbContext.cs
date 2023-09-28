using TaHooK.Api.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace TaHooK.Api.DAL;

public class TaHooKDbContext : DbContext
{
    public DbSet<AnswerEntity> Answers { get; set; } = null!;
    public DbSet<QuestionEntity> Questions { get; set; } = null!;
    public DbSet<QuizEntity> Quizes { get; set; } = null!;
    public DbSet<ScoreEntity> Scores { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    
    public TaHooKDbContext(DbContextOptions<TaHooKDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<QuizEntity>(entity =>
        {
            entity.HasMany(i => i.Questions)
                .WithOne(i => i.Quiz)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(i => i.Scores)
                .WithOne(i => i.Quiz)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<QuestionEntity>(entity =>
        {
            entity.HasMany(i => i.Answers)
                .WithOne(i => i.Question)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.Quiz)
                .WithMany(i => i.Questions)
                .OnDelete(DeleteBehavior.SetNull);
        });
        
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasMany(i => i.Scores)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AnswerEntity>(entity =>
        {
            entity.HasOne(i => i.Question)
                .WithMany(i => i.Answers)
                .OnDelete(DeleteBehavior.SetNull);
        });

    }
}