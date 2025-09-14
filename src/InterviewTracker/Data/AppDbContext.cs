using InterviewTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Interview> Interviews => Set<Interview>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<TechTopic> TechTopics => Set<TechTopic>();
    public DbSet<QuestionTopic> QuestionTopics => Set<QuestionTopic>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<QuestionTopic>().HasKey(x => new { x.QuestionId, x.TechTopicId });
        b.Entity<QuestionTopic>()
            .HasOne(qt => qt.Question).WithMany(q => q.Topics).HasForeignKey(qt => qt.QuestionId);
        b.Entity<QuestionTopic>()
            .HasOne(qt => qt.TechTopic).WithMany(t => t.Questions).HasForeignKey(qt => qt.TechTopicId);

        b.Entity<Company>().HasIndex(x => x.Name).IsUnique(false);
        b.Entity<TechTopic>().HasIndex(x => x.Slug).IsUnique(true);
    }
}
