using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.CourseUser.Exceptions;
using Domain.Courses;

using MediatR;

namespace Application.CourseUsers.Commands;

public record DeleteCourseUserCommand : IRequest<Result<Domain.Courses.CourseUser, CourseUserException>>
{
    public required Guid CourseUserId { get; init; }
}

public class DeleteCourseUserCommandHandler(ICourseUserRepository courseUserRepository)
    : IRequestHandler<DeleteCourseUserCommand, Result<Domain.Courses.CourseUser, CourseUserException>>
{
    public async Task<Result<Domain.Courses.CourseUser, CourseUserException>> Handle(
        DeleteCourseUserCommand request,
        CancellationToken cancellationToken)
    {
        var courseUserId = new CourseUserId(request.CourseUserId);

        var existingCourseUser = await courseUserRepository.GetById(courseUserId, cancellationToken);

        return await existingCourseUser.Match<Task<Result<Domain.Courses.CourseUser, CourseUserException>>>(
            async u => await DeleteEntity(u, cancellationToken),
            () => Task.FromResult<Result<Domain.Courses.CourseUser, CourseUserException>>(new CourseUserNotFoundException(courseUserId)));
    }

    public async Task<Result<Domain.Courses.CourseUser, CourseUserException>> DeleteEntity(Domain.Courses.CourseUser courseUser, CancellationToken cancellationToken)
    {
        try
        {
            return await courseUserRepository.Delete(courseUser, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUserUnknownException(courseUser.Id, exception);
        }
    }
}