using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models;

public class Company
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Name { get; set; } = default!;

    [MaxLength(200)]
    public string? Location { get; set; }

    public string? Notes { get; set; }

    public ICollection<Application> Applications { get; set; } = new List<Application>();

    public bool IsInactive { get; set; } = false;
}

public enum ApplicationStatus { Open, Rejected, Offer, Accepted }

public class Application
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    [Required, MaxLength(200)]
    public string Role { get; set; } = default!;

    [MaxLength(100)]
    public string? Seniority { get; set; }

    [MaxLength(100)]
    public string? Source { get; set; } // LinkedIn, Referral, Recruiter, etc.

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Open;
    public decimal? SalaryExpectation { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
}

public enum InterviewType
{
    Screen, HR, TechCoding, TechSystemDesign, TechArchitecture,
    PairProgramming, TakeHome, Managerial, Culture
}

public enum InterviewStage { Applied, Screening, Round1, Round2, Final, Offer }

public class Interview
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ApplicationId { get; set; }
    public Application? Application { get; set; }

    public InterviewStage Stage { get; set; } = InterviewStage.Round1;
    public InterviewType Type { get; set; } = InterviewType.TechCoding;

    public DateTime? Date { get; set; }
    public int? DurationMin { get; set; }
    public string? Outcome { get; set; } // Pass/Fail/Pending
    public string? Notes { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid InterviewId { get; set; }
    public Interview Interview { get; set; } = default!;

    [Required]
    public string Text { get; set; } = default!;

    [MaxLength(80)]
    public string? Category { get; set; } // dsa, system-design, web, devops, behavioral

    [MaxLength(20)]
    public string? Difficulty { get; set; } // easy/medium/hard

    public bool IsAskedByYou { get; set; } // întrebarea ta pt. ei
    public int? AnswerQuality { get; set; } // self-score 1..5

    public ICollection<QuestionTopic> Topics { get; set; } = new List<QuestionTopic>();
}

public class TechTopic
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(120)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(120)]
    public string Slug { get; set; } = default!;

    public ICollection<QuestionTopic> Questions { get; set; } = new List<QuestionTopic>();
}

public class QuestionTopic
{
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = default!;

    public Guid TechTopicId { get; set; }
    public TechTopic TechTopic { get; set; } = default!;
}
