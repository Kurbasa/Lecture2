using FluentValidation;

namespace Application.Course.Commands;

public class CreateCourseCommandValidator: AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
       
        RuleFor(x => x.Teacher).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}