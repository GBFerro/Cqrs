using Cqrs.Domain.Contracts.v1;
using Cqrs.Domain.Core.v1;
using MediatR;

namespace Cqrs.Domain.Commands.v1.DeletePerson;

public class DeletePersonCommandHandler : BaseHandler, IRequestHandler<DeletePersonCommand>
{
    private readonly IPersonRepository _repository;

    public DeletePersonCommandHandler(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        DeletePersonCommand command,
        CancellationToken cancellationToken)
    {
        await _repository.RemoveAsync(command.Id, cancellationToken);
    }
}
