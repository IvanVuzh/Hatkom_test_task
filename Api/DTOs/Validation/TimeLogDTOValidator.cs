using FluentValidation;

namespace Api.DTOs.Validation
{
    public class TimeLogDTOValidator : AbstractValidator<TimeLogDTO>
    {
        public TimeLogDTOValidator()
        {
            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThanOrEqualTo(x => x.StartTime + TimeSpan.FromMinutes(15)).WithMessage("Minimum time spent is 15 minutes");
        }
    }
}
