namespace Application.Common.Interfaces.Queries;
using Domain.Courses;
public interface ICourseQueries
{
    Task<IReadOnlyList<Course>> GetAll(CancellationToken cancellationToken);
}