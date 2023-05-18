using AutoMapper;
using Cqrs.Domain.Domain;
using Cqrs.Domain.Helpers;
using Microsoft.AspNetCore.Components.Forms;

namespace Cqrs.Domain.Queries.GetPerson;

public class GetPersonQueryProfile : Profile
{
    public GetPersonQueryProfile()
    {
        CreateMap<Person, GetPersonQueryResponse>()
            .ForMember(fileOutput => fileOutput.Cpf, option => option.MapFrom(input => input.Cpf.FormatCpf()));
    }
}
