using AutoMapper;
using Cqrs.Domain.Contracts.v1;
using Cqrs.Domain.Core.v1;
using Cqrs.Domain.Entities.v1;
using MediatR;

namespace Cqrs.Domain.Commands.v1.UpdatePerson;

public class UpdatePersonCommandHandler : BaseHandler, IRequestHandler<UpdatePersonCommand>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public UpdatePersonCommandHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdatePersonCommand command,
        CancellationToken cancellationToken
        )
    {
        var databaseEntity = await _repository.FindByIdAsync(command.Id, cancellationToken);

        if (databaseEntity is null)
        {
            AddNotification($"Person with Id = {command.Id} does not exist");
            SetStatusCode(System.Net.HttpStatusCode.NotFound);
            return;
        }

        var entity = _mapper.Map<Person>(command);
        entity.CreatedAt = databaseEntity.CreatedAt;

        await _repository.UpdateAsync(entity, cancellationToken);
        return;
    }
}
