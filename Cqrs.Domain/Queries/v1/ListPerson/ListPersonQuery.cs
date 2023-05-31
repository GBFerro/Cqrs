using MediatR;

namespace Cqrs.Domain.Queries.v1.ListPerson;

public class ListPersonQuery : IRequest<IEnumerable<ListPersonQueryResponse>>
{
    public ListPersonQuery(string? name, string? cpf)
    {
        Name = name;
        Cpf = cpf;
    }

    public string? Name { get; }
    public string? Cpf { get; }
}
