using AutoMapper;
using Cqrs.Domain.Contracts.v1;
using Cqrs.Domain.Core.v1;
using Cqrs.Domain.Entities.v1;
using MediatR;

namespace Cqrs.Domain.Commands.v1.CreatePerson;

public class CreatePersonCommandHandler : BaseHandler, IRequestHandler<CreatePersonCommand, Guid>
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;
    public CreatePersonCommandHandler(IPersonRepository personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Person>(request);
        await _personRepository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}
