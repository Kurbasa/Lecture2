using Application.CourseUser.Command;
using Application.CourseUsers.Commands;
using FluentValidation;

namespace Application.CourseUsers.Commands;

public class CreateCourseUserCommandValidator : AbstractValidator<CreateCourseUserCommand>
{
    public CreateCourseUserCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.InviteToCourse)
            .NotEmpty()
            .Must(BeUtc).WithMessage("FinishAt must be in UTC.");
        RuleFor(x => x.FinishCourse)
            .NotEmpty()
            .Must(BeUtc).WithMessage("FinishAt must be in UTC.");
        
    }
    private bool BeUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc;
    }
}