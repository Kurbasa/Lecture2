using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.CourseUser.Exceptions;
using Domain.Courses;
using Domain.Users;
using MediatR;

namespace Application.CourseUser.Command;

public record CreateCourseUserCommand : IRequest<Result<Domain.Courses.CourseUser, CourseUserException>>
{
    public Guid UserId { get; init; }
    public Guid CourseId { get; init; }
    public required string Raiting { get; init; }
    public required DateTime InviteToCourse { get; init; }
    public required DateTime FinishCourse { get; init; }
}

public class CreateCourseUserCommandHandler : IRequestHandler<CreateCourseUserCommand, Result<Domain.Courses.CourseUser, CourseUserException>>
{
    private readonly ICourseUserRepository _courseuserRepository;
    private readonly ICourseRepository _courseRepository;

    public CreateCourseUserCommandHandler(ICourseUserRepository courseuserRepository, ICourseRepository courseRepository)
    {
        _courseuserRepository = courseuserRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Result<Domain.Courses.CourseUser, CourseUserException>> Handle(
        CreateCourseUserCommand request,
        CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var userId = new UserId(request.UserId);
        var currentCourse = await _courseRepository.GetById(courseId, cancellationToken);
        var coursUsers = await _courseuserRepository.GetListCourseId(courseId, cancellationToken);
        
        var existingCourse = await _courseuserRepository.SearchByRaiting(request.Raiting, cancellationToken);
        
        var existingCourseUser = await _courseuserRepository.GetByCourseIdAndUserId(
            courseId, 
            userId, 
            cancellationToken);

        return await existingCourseUser.Match(
            f => Task.FromResult<Result<Domain.Courses.CourseUser, CourseUserException>>(
                new CourseUserAlreadyExistsException(f.Id)),
            async () =>
            {
                var currentCourse = await _courseRepository.GetById(courseId, cancellationToken);
        
                return await currentCourse.Match(
                    async c => 
                    {
                        if(coursUsers.Count < c.MaxStudents) 
                        {
                            return await CreateEntity(courseId, userId, request.Raiting, request.InviteToCourse, request.FinishCourse, cancellationToken);
                        }
                        return await Task.FromResult<Result<Domain.Courses.CourseUser, CourseUserException>>(
                            new CourseMaxStudentsException(courseId));
                    },
                    () => Task.FromResult<Result<Domain.Courses.CourseUser, CourseUserException>>(
                        new CourseNooFoundedException(courseId))
                );
            }
        );
    }

    private async Task<Result<Domain.Courses.CourseUser, CourseUserException>> CreateEntity(
        CourseId courseId, 
        UserId userId, 
        string raiting,
        DateTime inviteToCourse, 
        DateTime finishCourse,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Domain.Courses.CourseUser.New(CourseUserId.New(), courseId, userId, raiting, inviteToCourse, finishCourse);
            return await _courseuserRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUserUnknownException(CourseUserId.Empty, exception);
        }
    }
}
