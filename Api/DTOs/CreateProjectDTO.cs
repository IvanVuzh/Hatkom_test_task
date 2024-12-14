namespace Api.DTOs
{
    public class CreateProjectDTO
    {
        public string Name { get; set; } = null!;

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public TimeSpan? TimeSpent { get; set; } // questionable, maybe we will allow to added already started projects
    }
}
