using Cqrs.Domain.Commands.v1.CreatePerson;
using Cqrs.Domain.Commands.v1.DeletePerson;
using Cqrs.Domain.Commands.v1.UpdatePerson;
using Cqrs.Domain.Core.v1;
using Cqrs.Domain.Queries.v1.GetPerson;
using Cqrs.Domain.Queries.v1.ListPerson;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Api.Controllers.v1;

[ApiController]
[Route("api/v1/people")]
public class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeopleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}", Name = "Get Person By Id")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetPersonQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet(Name = "List People")]
    public async Task<IActionResult> GetAsync([FromQuery] string? name, [FromQuery] string? cpf, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ListPersonQuery(name, cpf), cancellationToken);
        return Ok(response);
    }

    [HttpPost(Name = "Insert Person")]
    public async Task<IActionResult> InsertAsync([FromBody] CreatePersonCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Created($"api/v1/people/{response}", new
        {
            id = response,
            command.Cpf,
            command.DateBirth,
            command.Email,
            command.Name
        });
    }

    [HttpPut("{id:guid}", Name = "Update Person")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdatePersonCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}", Name = "Delete Person By Id")]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePersonCommand(id), cancellationToken);
        return NoContent();
    }


    private IActionResult GetResponse<THandler, TResponse>(THandler handler, TResponse response)
        where THandler : BaseHandler
    {
        return StatusCode((int)handler.GetStatusCode(),
            new
            {
                Data = response,
                Notification = handler.GetNotifications()
            });
    }

    private IActionResult GetResponse<THandler>(THandler handler)
        where THandler : BaseHandler
    {
        return StatusCode((int)handler.GetStatusCode(),
            new
            {
                Notification = handler.GetNotifications()
            });
    }
}
