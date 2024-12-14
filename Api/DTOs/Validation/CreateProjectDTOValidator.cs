using Api.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Api.DTOs.Validation
{
    public class CreateProjectDTOValidator : AbstractValidator<CreateProjectDTO>
    {
        private readonly ProjectContext _context;

        public CreateProjectDTOValidator(ProjectContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(100).WithMessage("Project name must not exceed 100 characters.")
                .MustAsync(async (name, cancellation) => await IsUniqueName(name)).WithMessage("Project name must be unique.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.");
        }

        private async Task<bool> IsUniqueName(string name) 
        { 
            return !await _context.Projects.AnyAsync(p => p.Name == name); 
        }
    }
}
