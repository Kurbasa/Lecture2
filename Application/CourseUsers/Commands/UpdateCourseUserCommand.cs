using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.CourseUser.Exceptions;
using Domain.Courses;
using Domain.Users;
using MediatR;
using Optional;

namespace Application.CourseUsers.Commands;

public record UpdateCourseUserCommand : IRequest<Result<Domain.Courses.CourseUser, CourseUserException>>
{
    public required Guid Id { get; init; }
    public required Guid CourseId { get; init; }
    public required Guid UserId { get; init; }
    public required string Raiting { get; init;  }   
    public required DateTime InviteToCourse { get; init; }
    public required DateTime FinishCourse { get; init; }
}

public class UpdateCourseUserCommandHandler(
    ICourseUserRepository courseUserRepository) : IRequestHandler<UpdateCourseUserCommand, Result<Domain.Courses.CourseUser, CourseUserException>>
{
    public async Task<Result<Domain.Courses.CourseUser, CourseUserException>> Handle(
        UpdateCourseUserCommand request,
        CancellationToken cancellationToken)
    {
        var courseId = new CourseUserId(request.Id);
        var course = await courseUserRepository.GetById(courseId, cancellationToken);

        return await course.Match(
            async f =>
            {
                var existingCourseUser = await CheckDuplicated(courseId, cancellationToken);

                return await existingCourseUser.Match(
                    ef => Task.FromResult<Result<Domain.Courses.CourseUser, CourseUserException>>(new CourseUserAlreadyExistsException(ef.Id)),
                    async () => await UpdateEntity(f, new CourseId(request.CourseId), new UserId(request.UserId) ,request.Raiting,request.InviteToCourse,request.FinishCourse, cancellationToken));
            },
            () => Task.FromResult<Result<Domain.Courses.CourseUser, CourseUserException>>(new CourseUserNotFoundException(courseId)));
    }

    private async Task<Result<Domain.Courses.CourseUser, CourseUserException>> UpdateEntity(
        Domain.Courses.CourseUser course,
        CourseId courseId,
        UserId userId,
        string raiting,
        DateTime inviteToCourse,
        DateTime finishCourse,
        CancellationToken cancellationToken)
    {
        try
        {
            course.UpdateDetails(courseId,userId, raiting, inviteToCourse,finishCourse);

            return await courseUserRepository.Update(course, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUserUnknownException(course.Id, exception);
        }
    }

    private async Task<Option<Domain.Courses.CourseUser>> CheckDuplicated(
        CourseUserId courseId,
        CancellationToken cancellationToken)
    {
        var course = await courseUserRepository.GetById(courseId, cancellationToken);

        return course.Match(
            f => f.Id == courseId ? Option.None<Domain.Courses.CourseUser>() : Option.Some(f), 
            Option.None<Domain.Courses.CourseUser>);
    }
}