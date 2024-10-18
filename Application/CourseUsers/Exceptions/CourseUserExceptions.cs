using Application.Course.Exceptions;

namespace Application.CourseUser.Exceptions;

using Domain.Courses;

public abstract class CourseUserException : Exception
{
    public CourseId? CourseId { get; }
    public CourseUserId? CourseUserId { get; }
    protected CourseUserException(CourseId? courseId, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        CourseId = courseId;
    }
    protected CourseUserException(CourseUserId? courseUserId, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        CourseUserId = courseUserId;
    }
}
public class CourseUserNotFoundException(CourseUserId id) : CourseUserException(id, $"Course under id: {id} not found");

public class CourseUserAlreadyExistsException(CourseUserId id) : CourseUserException(id, $"Course already exists: {id}");

public class CourseUserUnknownException(CourseUserId id, Exception innerException)
    : CourseUserException(id, $"Unknown exception for the Course under id: {id}", innerException);

    
public class CourseMaxStudentsException(CourseId id) : CourseUserException(id, $"Course already exists: {id}");
public class CourseNooFoundedException(CourseId id) : CourseUserException(id, $"Course no founded");