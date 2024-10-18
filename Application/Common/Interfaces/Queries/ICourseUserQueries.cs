using Domain.Courses;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;


public interface ICourseUserQueries
{
    Task<IReadOnlyList<Domain.Courses.CourseUser>> GetAll(CancellationToken cancellationToken);
    Task<Option<Domain.Courses.CourseUser>> GetById(CourseUserId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Domain.Courses.Course>> GetAllCourses(CourseId courseId, CancellationToken cancellationToken);
}