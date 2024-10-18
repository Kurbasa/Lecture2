using Domain.Courses;

namespace Api.Dtos;

public record CourseUserDto(
    Guid? Id,
    Guid? CourseId,
    Guid? UserId,
    string Rating,
    DateTime InviteToCourse,
    DateTime FinishCourse,
    bool IsApproved)
{
    public static CourseUserDto FromDomainModel(CourseUser courseUser)
        => new(
            Id: courseUser.Id.Value,
            CourseId: courseUser.CourseId.Value,
            UserId: courseUser.UserId.Value,
            Rating: courseUser.Rating,
            InviteToCourse: courseUser.InviteToCourse,
            FinishCourse: courseUser.FinishCourse,
            IsApproved: courseUser.IsApproved
        );
}