using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models
{
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
}
