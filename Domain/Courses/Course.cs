namespace Domain.Courses;

public class Course
{
    public CourseId Id { get; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxStudents { get; set; }
    public string Teacher { get; set; }
    
    
    private Course(CourseId id, string name, DateTime startDate, DateTime endDate, int maxStudents, string teacher)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        MaxStudents = maxStudents;
        Teacher = teacher;
        
    }
    public static Course New(CourseId id, string name, DateTime startDate, DateTime endDate, int maxStudents, string teacher)
        => new(id, name, startDate, endDate, maxStudents, teacher);
    
    
    public void UpdateDetails(string name,DateTime? staTime,DateTime? endDate, int maxStudents,string teacher)
    {
        Name = name;
        StartDate = (DateTime)staTime;
        EndDate = (DateTime)endDate;
        MaxStudents = maxStudents;
        Teacher = teacher;
    }
}