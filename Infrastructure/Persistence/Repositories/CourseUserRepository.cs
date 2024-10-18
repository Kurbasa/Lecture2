using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CourseUserRepository(ApplicationDbContext context): ICourseUserRepository, ICourseUserQueries
{
    public async Task<IReadOnlyList<CourseUser>> GetAll(CancellationToken cancellationToken)
    {
        return await context.CourseUsers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<CourseUser>> SearchByRaiting(string raiting,CancellationToken cancellationToken)
    {
        var entity = await context.CourseUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Rating == raiting, cancellationToken);

        return entity == null ? Option.None<CourseUser>() : Option.Some(entity);
    }
    public async Task<IReadOnlyList<Course>> GetAllCourses(CourseId courseId,CancellationToken cancellationToken)
    {
        return await context.Courses
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<CourseUser>> GetById(CourseUserId id, CancellationToken cancellationToken)
    {
        var entity = await context.CourseUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<CourseUser>() : Option.Some(entity);
    }
    
    public async Task<List<CourseUser>> GetListCourseId(CourseId courseId, CancellationToken cancellationToken)
    {
        var  entity = await context.CourseUsers
            .AsNoTracking()
            .Where(x => x.CourseId == courseId)
            .ToListAsync(cancellationToken);
        return entity;
    }

    public async Task<Option<CourseUser>> GetbyUserOrCourseId(CourseId courseId, UserId userId, CancellationToken cancellationToken)
    {
        var entity = await context.CourseUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId, cancellationToken);

        return entity == null ? Option.None<CourseUser>() : Option.Some(entity);
    }
    public async Task<Option<CourseUser>> GetByCourseIdAndUserId(CourseId courseId, UserId userId, CancellationToken cancellationToken)
    {
        var entity = await context.CourseUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId, cancellationToken);

        return entity == null ? Option.None<CourseUser>() : Option.Some(entity);
    }
    public async Task<CourseUser> Add(CourseUser courseUser, CancellationToken cancellationToken)
    {
        await context.CourseUsers.AddAsync(courseUser, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return courseUser;
    }

    public async Task<CourseUser> Update(CourseUser courseUser, CancellationToken cancellationToken)
    {
        context.CourseUsers.Update(courseUser);

        await context.SaveChangesAsync(cancellationToken);

        return courseUser;
    }
    public async Task<int> CountStudentsInCourse(CourseId courseId, CancellationToken cancellationToken)
    {
        return await context.CourseUsers
            .AsNoTracking()
            .CountAsync(x => x.CourseId == courseId, cancellationToken);
    }
    public async Task<CourseUser> Delete(CourseUser courseUser, CancellationToken cancellationToken)
    {
        context.CourseUsers.Remove(courseUser);
        await context.SaveChangesAsync(cancellationToken);
        return courseUser;
    }
    
}