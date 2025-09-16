using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models
{
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
}
