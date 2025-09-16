namespace InterviewTracker.Models
{
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

        public bool IsDeleted { get; set; } = false;
    }
}
