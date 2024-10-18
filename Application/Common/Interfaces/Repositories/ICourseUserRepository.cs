using Domain.Courses;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICourseUserRepository
{
    Task<Option<Domain.Courses.CourseUser>> GetById(CourseUserId id, CancellationToken cancellationToken);
    Task <List<Domain.Courses.CourseUser>> GetListCourseId(CourseId courseId, CancellationToken cancellationToken); 
    Task<Option<Domain.Courses.CourseUser>> SearchByRaiting(string name, CancellationToken cancellationToken);
    Task<Option<Domain.Courses.CourseUser>> GetByCourseIdAndUserId(CourseId courseId, UserId userId, CancellationToken cancellationToken);
    Task<Domain.Courses.CourseUser> Add(Domain.Courses.CourseUser courseuser, CancellationToken cancellationToken);
    Task<Domain.Courses.CourseUser> Update(Domain.Courses.CourseUser courseuser, CancellationToken cancellationToken);
    Task<Domain.Courses.CourseUser> Delete(Domain.Courses.CourseUser courseuser, CancellationToken cancellationToken);
}