using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.Seeds;

namespace TaHooK.Api.DAL;

public class TaHooKDbContext : DbContext
{
    private readonly bool _seedDemoData;

    public TaHooKDbContext(DbContextOptions<TaHooKDbContext> options, bool seedDemoData = false)
        : base(options)
    {
        _seedDemoData = seedDemoData;
    }

    public DbSet<AnswerEntity> Answers { get; set; } = null!;
    public DbSet<QuestionEntity> Questions { get; set; } = null!;
    public DbSet<QuizEntity> Quizes { get; set; } = null!;
    public DbSet<QuizTemplateEntity> QuizTemplates { get; set; } = null!;
    public DbSet<ScoreEntity> Scores { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<QuizTemplateEntity>(entity =>
        {
            entity.HasMany(i => i.Questions)
                .WithOne(i => i.QuizTemplate)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<QuizEntity>(entity =>
        {
            entity.HasMany(i => i.Scores)
                .WithOne(i => i.Quiz)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<QuestionEntity>(entity =>
        {
            entity.HasMany(i => i.Answers)
                .WithOne(i => i.Question)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasMany(i => i.Scores)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(i => i.Quizes)
                .WithOne(i => i.Creator)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(i => i.QuizTemplates)
                .WithOne(i => i.Creator)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
    
    public async Task SeedDatabaseAsync()
    {
        if (!_seedDemoData)
        {
            return;
        }
        UserSeeds.Seed(this);
        QuizTemplateSeeds.Seed(this);
        QuizSeeds.Seed(this);
        QuestionSeeds.Seed(this);
        AnswerSeeds.Seed(this);
        ScoreSeeds.Seed(this);
        
        await SaveChangesAsync();
    }
}