namespace InterviewTracker.Models
{
    public class QuestionTopic
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = default!;

        public Guid TechTopicId { get; set; }
        public TechTopic TechTopic { get; set; } = default!;
    }
}
