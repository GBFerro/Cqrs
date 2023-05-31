using MediatR;

namespace Cqrs.Domain.Commands.v1.DeletePerson;

public class DeletePersonCommand : IRequest
{
    public DeletePersonCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
