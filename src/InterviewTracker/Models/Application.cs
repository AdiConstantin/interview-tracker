using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models
{
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

        public bool IsDeleted { get; set; } = false;
    }
}
