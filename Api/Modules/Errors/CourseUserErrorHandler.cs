using Application.Course.Exceptions;
using Application.CourseUser.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CourseUserErrorHandler
{
    public static ObjectResult ToObjectResult(this CourseUserException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                CourseUserNotFoundException => StatusCodes.Status404NotFound,
                CourseUserAlreadyExistsException => StatusCodes.Status409Conflict,
                CourseUserUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Course error handler does not implemented")
            }
        };
    }
}