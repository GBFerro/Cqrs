using AutoMapper;
using Cqrs.Domain.Contracts;
using Cqrs.Domain.Core;

namespace Cqrs.Domain.Queries.GetPerson;

public class GetPersonQueryHandler : BaseHandler
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public GetPersonQueryHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetPersonQueryResponse> HandleAsync(
        GetPersonQuery personQuery, 
        CancellationToken cancellationToken)
    {
        var person = await _repository.GetByIdAsync(personQuery.Id, cancellationToken);

        if(!string.IsNullOrWhiteSpace(person?.Name))
            return _mapper.Map<GetPersonQueryResponse>(person);

        AddNotification($"Person with Id = {personQuery.Id} does not exist.");
        SetStatusCode(System.Net.HttpStatusCode.NotFound);

        return null;
    }
}
