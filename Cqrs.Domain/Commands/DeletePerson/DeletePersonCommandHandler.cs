using Cqrs.Domain.Contracts;
using Cqrs.Domain.Core;

namespace Cqrs.Domain.Commands.DeletePerson;

public class DeletePersonCommandHandler : BaseHandler
{
    private readonly IPersonRepository _repository;

    public DeletePersonCommandHandler(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(
        DeletePersonCommand command, 
        CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(command.Id, cancellationToken);
    }
}
