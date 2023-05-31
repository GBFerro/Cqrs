using AutoMapper;
using Cqrs.Domain.Entities.v1;
using Cqrs.Domain.Helpers.v1;
using Microsoft.AspNetCore.Components.Forms;

namespace Cqrs.Domain.Queries.v1.GetPerson;

public class GetPersonQueryProfile : Profile
{
    public GetPersonQueryProfile()
    {
        CreateMap<Person, GetPersonQueryResponse>()
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
