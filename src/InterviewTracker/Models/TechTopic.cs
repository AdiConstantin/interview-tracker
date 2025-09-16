using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models
{
    public class TechTopic
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(120)]
        public string Name { get; set; } = default!;

        [Required, MaxLength(120)]
        public string Slug { get; set; } = default!;

        public ICollection<QuestionTopic> Questions { get; set; } = new List<QuestionTopic>();
    }
}
