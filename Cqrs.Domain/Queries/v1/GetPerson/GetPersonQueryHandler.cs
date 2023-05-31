using AutoMapper;
using Cqrs.Domain.Contracts.v1;
using Cqrs.Domain.Core.v1;
using MediatR;

namespace Cqrs.Domain.Queries.v1.GetPerson;

public class GetPersonQueryHandler : BaseHandler, IRequestHandler<GetPersonQuery, GetPersonQueryResponse>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public GetPersonQueryHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetPersonQueryResponse> Handle(
        GetPersonQuery personQuery,
        CancellationToken cancellationToken)
    {
        var databaseEntity = await _repository.FindByIdAsync(personQuery.Id, cancellationToken);

        if (databaseEntity is not null)
            return _mapper.Map<GetPersonQueryResponse>(databaseEntity);

        AddNotification($"Person with Id = {personQuery.Id} does not exist.");
        SetStatusCode(System.Net.HttpStatusCode.NotFound);

        return null;
    }
}
