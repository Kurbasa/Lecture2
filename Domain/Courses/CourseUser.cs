using Domain.Faculties;
using Domain.Users;

namespace Domain.Courses;

public class CourseUser
{
    public CourseUserId Id { get; }
    
    public CourseId CourseId { get; private set; }
    public Course? Course { get; private set; }
    
    public UserId UserId { get; private set;  }
    public User? User { get; private set;  } 
    public string Rating { get; private set;  }   
    public DateTime InviteToCourse { get; private set; }
    public DateTime FinishCourse { get; private set; }
    
    public bool IsApproved { get; private set; }
    
    public CourseUser(CourseUserId id, CourseId courseId, UserId userId, string rating, DateTime inviteToCourse, DateTime finishCourse, bool isApproved)
    {
        Id = id;
        CourseId = courseId;
        UserId = userId;
        Rating = rating;
        InviteToCourse = inviteToCourse;
        FinishCourse = finishCourse;
        IsApproved = isApproved;
    }

    
    public static CourseUser New(CourseUserId id, CourseId courseId, UserId userId, string rating, DateTime inviteToCourse, DateTime finishCourse)
    {
        return new CourseUser(id, courseId, userId, rating, inviteToCourse, finishCourse, true);
    }
    
    public void UpdateDetails(CourseId courseId, UserId userId, string rating, DateTime invitetocourse, DateTime finishcourse)
    {
        CourseId = courseId;
        UserId = userId;
        Rating = rating;
        InviteToCourse = invitetocourse;
        FinishCourse = finishcourse;
    }
    
    public void CLoseCourse()
    {
        IsApproved = false;
    }
    
}