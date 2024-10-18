using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Course.Exceptions;
using Domain.Courses;
using MediatR;

namespace Application.Course.Commands;

public record CreateCourseCommand : IRequest<Result<Domain.Courses.Course, CourseException>>
{
    public required string Name { get; init; }
    public required int MaxStudents { get; init; }
    public required string Teacher { get; init; }
    public required DateTime? StartDate{ get; init; }
    public required DateTime? EndDate{ get; init; }
    
    
}

public class CreateCourseCommandHandler(
    ICourseRepository courseRepository) : IRequestHandler<CreateCourseCommand, Result<Domain.Courses.Course, CourseException>>
{
    public async Task<Result<Domain.Courses.Course, CourseException>> Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var existingCourse = await courseRepository.SearchByName(request.Name,cancellationToken);

        return await existingCourse.Match(
            f => Task.FromResult<Result<Domain.Courses.Course, CourseException>>(new CourseAlreadyExistsException(f.Id)),
            async () => await CreateEntity(request.Name,request.MaxStudents,request.Teacher,request.StartDate,request.EndDate, cancellationToken));
    }

    private async Task<Result<Domain.Courses.Course, CourseException>> CreateEntity(
        string name,
        int maxStudents,
        string teacger,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Domain.Courses.Course.New(
                CourseId.New(),
                name,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMonths(1),
                maxStudents ,
                teacger
            );

            return await courseRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUnknownException(CourseId.Empty, exception);
        }
    }

}