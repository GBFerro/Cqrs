using AutoMapper;
using Cqrs.Domain.Domain;
using Cqrs.Domain.Helpers;

namespace Cqrs.Domain.Commands.UpdatePerson;

public class UpdatePersonCommandProfile : Profile
{
    public UpdatePersonCommandProfile()
    {
        CreateMap<UpdatePersonCommand, Person>()
            .ForMember(fileOutput => fileOutput.Cpf, options => options
            .MapFrom(input => input.Cpf.RemoveMaskCpf()))

            .ForMember(fileOutput => fileOutput.Name, options => options
            .MapFrom(input => input.Name.ToUpper()))

            .ForMember(fileOutput => fileOutput.Email, options => options
            .MapFrom(input => input.Email.ToUpper()));
    }
}
