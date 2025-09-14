using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Seed;

public static class SeedData
{
    public static async Task EnsureSeedAsync(AppDbContext db)
    {
        if (await db.Companies.AnyAsync()) return;

        var c = new Company { Name = "Company-A", Location = "Remote" };
        var app = new Application
        {
            Company = c,
            Role = ".NET Senior Developer",
            Seniority = "Senior",
            Source = "LinkedIn",
            Status = ApplicationStatus.Open
        };
        var iv1 = new Interview
        {
            Application = app,
            Stage = InterviewStage.Screening,
            Type = InterviewType.Screen,
            Notes = "Intro call"
        };
        var iv2 = new Interview
        {
            Application = app,
            Stage = InterviewStage.Round1,
            Type = InterviewType.TechCoding,
            Notes = "HackerRank live"
        };

        var t1 = new TechTopic { Name = "System Design", Slug = "system-design" };
        var t2 = new TechTopic { Name = "Microservices", Slug = "microservices" };
        var t3 = new TechTopic { Name = "HTTP Lifecycle", Slug = "http-lifecycle" };

        var q1 = new Question { Interview = iv2, Text = "Two Sum / DSA", Category = "dsa", Difficulty = "easy" };
        var q2 = new Question { Interview = iv2, Text = "Ce se întâmplă când tastezi un URL și apeși Enter?", Category = "web", Difficulty = "medium" };
        var q3 = new Question { Interview = iv2, Text = "Idempotency & Outbox pattern", Category = "system-design", Difficulty = "medium" };

        db.AddRange(c, app, iv1, iv2, t1, t2, t3, q1, q2, q3);
        await db.SaveChangesAsync();

        db.QuestionTopics.AddRange(
            new QuestionTopic { QuestionId = q2.Id, TechTopicId = t3.Id },
            new QuestionTopic { QuestionId = q3.Id, TechTopicId = t2.Id },
            new QuestionTopic { QuestionId = q3.Id, TechTopicId = t1.Id }
        );

        await db.SaveChangesAsync();
    }
}
