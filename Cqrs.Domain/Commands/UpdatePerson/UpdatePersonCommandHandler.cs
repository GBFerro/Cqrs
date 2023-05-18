using AutoMapper;
using Cqrs.Domain.Commands.CreatePerson;
using Cqrs.Domain.Contracts;
using Cqrs.Domain.Core;
using Cqrs.Domain.Domain;

namespace Cqrs.Domain.Commands.UpdatePerson;

public class UpdatePersonCommandHandler : BaseHandler
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public UpdatePersonCommandHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> HandleAsync(
        UpdatePersonCommand command,
        CancellationToken cancellationToken
        )
    {
        var databaseEntity = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (string.IsNullOrWhiteSpace(databaseEntity?.Name))
        {
            AddNotification($"Person with Id = {command.Id} does not exist");
            SetStatusCode(System.Net.HttpStatusCode.NotFound);
            return Guid.Empty;
        }

        var entity = _mapper.Map<Person>(command);
        entity.CreatedAt = databaseEntity.CreatedAt;

        await _repository.UpdateAsync(entity, cancellationToken);
        return entity.Id;
    }
}
