using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Course.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("courses")]
[ApiController]
public class CourseController(ISender sender, ICourseQueries facultyQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseDtos>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await facultyQueries.GetAll(cancellationToken);

        return entities.Select(CourseDtos.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<CourseDtos>> Create(
        [FromBody] CourseDtos request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCourseCommand
        {
            Name = request.Name,
            MaxStudents = request.MaxStudents,
            Teacher = request.Teacher,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseDtos>>(
            f => CourseDtos.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken)
    {
        var input = new DeleteCourseCommand
        {
            CourseId = id
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult>(
            _ => NoContent(),
            e => e.ToObjectResult());

    }
    [HttpPut]
    public async Task<ActionResult<CourseDtos>> Update(
        [FromBody] CourseDtos request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCourseCommand
        {
            Name = request.Name,
            CourseId = request.Id!.Value,
            StartAt = request.StartDate,
            FinishAt = request.EndDate,
            MaxStudents = request.MaxStudents,
            Teacher = request.Teacher
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseDtos>>(
            f => CourseDtos.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
}