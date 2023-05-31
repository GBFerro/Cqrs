using AutoMapper;
using Cqrs.Domain.Contracts.v1;
using Cqrs.Domain.Core.v1;
using MediatR;

namespace Cqrs.Domain.Queries.v1.ListPerson;

public class ListPersonQueryHandler : BaseHandler, IRequestHandler<ListPersonQuery, IEnumerable<ListPersonQueryResponse>>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public ListPersonQueryHandler(IPersonRepository personRepository, IMapper mapper)
    {
        _repository = personRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ListPersonQueryResponse>> Handle(ListPersonQuery query, CancellationToken cancellationToken)
    {
        var people = await _repository.FindAsync(
            person =>
            (query.Name == null || person.Name.Value.Contains(query.Name.ToUpper()))
            && (query.Cpf == null || person.Cpf.Value.Contains(query.Cpf)),
            cancellationToken
            );

        return _mapper.Map<IEnumerable<ListPersonQueryResponse>>(people);
    }
}
