using FluentValidation;

namespace Application.Courses.Commands;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
            .MinimumLength(3);

        RuleFor(x => x.MaxStudents)
            .NotEmpty()
            .InclusiveBetween(0, 100)
            .WithMessage("MaxStudents must be a number between 0 and 100.");
    }
}