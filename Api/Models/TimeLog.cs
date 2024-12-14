namespace Api.Models
{
    public class TimeLog
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
    }
}
