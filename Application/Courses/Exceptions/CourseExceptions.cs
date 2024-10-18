using Domain.Courses;

namespace Application.Course.Exceptions;

public abstract class CourseException(CourseId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public CourseId CourseId { get; } = id;
}

public class CourseNotFoundException(CourseId id) : CourseException(id, $"Course under id: {id} not found");

public class CourseAlreadyExistsException(CourseId id) : CourseException(id, $"Course already exists: {id}");

public class CourseUnknownException(CourseId id, Exception innerException)
    : CourseException(id, $"Unknown exception for the course under id: {id}", innerException);