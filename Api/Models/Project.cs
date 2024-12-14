namespace Api.Models
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public TimeSpan TimeSpent { get; set; } = TimeSpan.Zero;

        public bool IsCompleted { get; set; } = false;
    }
}
