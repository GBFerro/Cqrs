using Cqrs.Domain.Commands.CreatePerson;
using Cqrs.Domain.Commands.DeletePerson;
using Cqrs.Domain.Commands.UpdatePerson;
using Cqrs.Domain.Core;
using Cqrs.Domain.Domain;
using Cqrs.Domain.Queries.GetPerson;
using Cqrs.Domain.Queries.ListPerson;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly CreatePersonCommandHandler _createPersonCommandHandler;
    private readonly UpdatePersonCommandHandler _updatePersonCommandHandler;
    private readonly DeletePersonCommandHandler _deletePersonCommandHandler;
    private readonly ListPersonQueryHandler _listPersonQueryHandler;
    private readonly GetPersonQueryHandler _getPersonQueryHandler;

    public PeopleController(
        CreatePersonCommandHandler createPersonCommandHandler,
        ListPersonQueryHandler listPersonQueryHandler,
        GetPersonQueryHandler getPersonQueryHandler,
        UpdatePersonCommandHandler updatePersonCommandHandler,
        DeletePersonCommandHandler deletePersonCommandHandler)
    {
        this._createPersonCommandHandler = createPersonCommandHandler;
        this._listPersonQueryHandler = listPersonQueryHandler;
        this._getPersonQueryHandler = getPersonQueryHandler;
        this._updatePersonCommandHandler = updatePersonCommandHandler;
        this._deletePersonCommandHandler = deletePersonCommandHandler;
    }

    [HttpPost(Name = "Insert Person")]
    public async Task<IActionResult> InsertPersonAsync(
        CreatePersonCommand createPersonCommand,
        CancellationToken cancellationToken)
    {
        var result = await _createPersonCommandHandler
            .HandleAsync(createPersonCommand, cancellationToken);

        return GetResponse(_createPersonCommandHandler, result);
    }

    [HttpGet(Name = "List People")]
    public async Task<IActionResult> GetPeopleAsync(
        [FromQuery] string? name,
        [FromQuery] string? cpf,
        CancellationToken cancellationToken)
    {
        var result = await _listPersonQueryHandler
            .HandleAsync(
            new ListPersonQuery(name, cpf), 
            cancellationToken);

        return GetResponse(_listPersonQueryHandler, result);
    }

    [HttpGet("{id:Guid}", Name = "Get Person By Id")]
    public async Task<IActionResult> GetPersonByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _getPersonQueryHandler
            .HandleAsync(
            new GetPersonQuery(id), 
            cancellationToken);

        return GetResponse(_getPersonQueryHandler, result);
    }

    [HttpPut("{id:Guid}", Name = "Update Person")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePersonCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await _updatePersonCommandHandler.HandleAsync(
            command,
            cancellationToken);

        return GetResponse(_updatePersonCommandHandler, result);
    }

    [HttpDelete("{id:Guid}", Name = "Delete Person")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        await _deletePersonCommandHandler
            .HandleAsync(
            new DeletePersonCommand(id), 
            cancellationToken);

        return GetResponse(_deletePersonCommandHandler);
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