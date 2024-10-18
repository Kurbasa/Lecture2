using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.CourseUser.Command;
using Application.CourseUsers.Commands;
using Domain.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("courseUsers")]
[ApiController]
public class CourseUsersController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICourseUserQueries _courseUserQueries;

    public CourseUsersController(ISender sender, ICourseUserQueries courseUserQueries)
    {
        _sender = sender;
        _courseUserQueries = courseUserQueries;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseUserDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await _courseUserQueries.GetAll(cancellationToken);
        return Ok(entities.Select(CourseUserDto.FromDomainModel).ToList());
    }

    [HttpGet("{courseUserId:guid}")]
    public async Task<ActionResult<CourseUserDto>> Get([FromRoute] Guid courseUserId, CancellationToken cancellationToken)
    {
        var entity = await _courseUserQueries.GetById(new CourseUserId(courseUserId), cancellationToken);

        return entity.Match<ActionResult<CourseUserDto>>(
            u => Ok(CourseUserDto.FromDomainModel(u)),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CourseUserDto>> Create(
        [FromBody] CourseUserDto request,
        CancellationToken cancellationToken)
    {
        
        var input = new CreateCourseUserCommand
        {
            CourseId = request.CourseId.Value,
            UserId = request.UserId.Value,
            Raiting = request.Rating,
            InviteToCourse = request.InviteToCourse,
            FinishCourse = request.FinishCourse,
        };

        var result = await _sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            f => CreatedAtAction(nameof(Get), new { courseUserId = f.Id }, CourseUserDto.FromDomainModel(f)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{courseUserId:guid}")]
    public async Task<ActionResult<CourseUserDto>> Delete([FromRoute] Guid courseUserId, CancellationToken cancellationToken)
    {
        var input = new DeleteCourseUserCommand
        {
            CourseUserId = courseUserId
        };

        var result = await _sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            u => Ok(CourseUserDto.FromDomainModel(u)),
            e => e.ToObjectResult());
    }
    [HttpPut]
    public async Task<ActionResult<CourseUserDto>> Update(
        [FromBody] CourseUserDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCourseUserCommand
        {
            Id = request.Id!.Value,
            CourseId = request.CourseId!.Value,
            UserId = request.UserId!.Value,
            Raiting = request.Rating,
            InviteToCourse = request.InviteToCourse,
            FinishCourse = request.FinishCourse
        };

        var result = await _sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            f => CourseUserDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
}
