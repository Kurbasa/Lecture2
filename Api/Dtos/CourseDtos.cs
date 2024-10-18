using Domain.Courses;


namespace Api.Dtos;

public record CourseDtos(Guid? Id, string Name, int MaxStudents, string Teacher,DateTime? StartDate, DateTime? EndDate)
{
    public static CourseDtos FromDomainModel(Course course)
        => new(course.Id.Value, course.Name,course.MaxStudents,course.Teacher,course.StartDate,course.EndDate);
}