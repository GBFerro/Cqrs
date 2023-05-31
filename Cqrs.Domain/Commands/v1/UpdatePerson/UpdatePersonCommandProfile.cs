using AutoMapper;
using Cqrs.Domain.Commands.v1.CreatePerson;
using Cqrs.Domain.Entities.v1;
using Cqrs.Domain.Helpers.v1;
using Cqrs.Domain.ValueObjects.v1;

namespace Cqrs.Domain.Commands.v1.UpdatePerson;

public class UpdatePersonCommandProfile : Profile
{
    public UpdatePersonCommandProfile()
    {
        CreateMap<UpdatePersonCommand, Person>()
            .ForMember(
            fieldOutput => fieldOutput.Cpf,
            option => option.MapFrom(input => new Document(input.Cpf!)))

            .ForMember(
            fieldOutput => fieldOutput.Name,
            option => option.MapFrom(input => new Name(input.Name!)))

            .ForMember(
            fieldOutput => fieldOutput.Email,
            option => option.MapFrom(input => new Email(input.Email!)));
    }
}
