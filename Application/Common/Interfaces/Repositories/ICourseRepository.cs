using Optional;

namespace Application.Common.Interfaces.Repositories;
using Domain.Courses;
public interface ICourseRepository
{
    Task<Option<Course>> GetById(CourseId id, CancellationToken cancellationToken);
    Task<Option<Course>> SearchByName(string name,CancellationToken cancellationToken);
    Task<Course> Update(Course course, CancellationToken cancellationToken);
    Task<Course> Add(Course course, CancellationToken cancellationToken);
    Task<Course> Delete(Course course, CancellationToken cancellationToken);
}