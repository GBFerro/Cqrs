using AutoMapper;
using Cqrs.Domain.Entities.v1;
using Cqrs.Domain.Helpers.v1;

namespace Cqrs.Domain.Queries.v1.ListPerson;

public class ListPersonQueryProfile : Profile
{
    public ListPersonQueryProfile()
    {
        CreateMap<Person, ListPersonQueryResponse>()
            .ForMember(
            fileOutput => fileOutput.Cpf,
            option => option.MapFrom(
                input => input.Cpf.Value.FormatCpf()))

            .ForMember(
            fileOutput => fileOutput.Name,
            option => option.MapFrom(
                input => input.Name.Value))

            .ForMember(
            fileOutput => fileOutput.Email,
            option => option.MapFrom(
                input => input.Email.Value));
    }
}
