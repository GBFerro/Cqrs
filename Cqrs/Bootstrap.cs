using Cqrs.Domain.Commands.CreatePerson;
using Cqrs.Domain.Commands.DeletePerson;
using Cqrs.Domain.Commands.UpdatePerson;
using Cqrs.Domain.Contracts;
using Cqrs.Domain.Queries.GetPerson;
using Cqrs.Domain.Queries.ListPerson;
using Cqrs.Repository;
using Cqrs.Repository.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MongoDB.Driver;

namespace Cqrs.Api;

public static class Bootstrap
{
    public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories(configuration);
        services.AddCommands();
        services.AddQueries();
        services.AddMappers();
        services.AddValidators();
    }

    public static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddScoped<IValidator<CreatePersonCommand>, CreatePersonCommandValidator>();
        services.AddScoped<IValidator<UpdatePersonCommand>, UpdatePersonCommandValidator>();
    }

    private static void AddCommands(this IServiceCollection services)
    {
        services.AddTransient<CreatePersonCommandHandler>();
        services.AddTransient<UpdatePersonCommandHandler>();
        services.AddTransient<DeletePersonCommandHandler>();
    }

    private static void AddMappers(this IServiceCollection services) => 
        services.AddAutoMapper(
            typeof(ListPersonQueryProfile), 
            typeof(GetPersonQueryProfile),
            typeof(CreatePersonCommandProfile),
            typeof(UpdatePersonCommandProfile));

    private static void AddQueries(this IServiceCollection services)
    {
        services.AddTransient<ListPersonQueryHandler>();
        services.AddTransient<GetPersonQueryHandler>();
    }

    private static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoSettings = configuration.GetSection(nameof(MongoRepositorySettings));
        var clientSettings = MongoClientSettings.FromConnectionString(mongoSettings.Get<MongoRepositorySettings>().ConnectionString);

        services.Configure<MongoRepositorySettings>(mongoSettings);
        services.AddSingleton<IMongoClient>(new MongoClient(clientSettings));
        services.AddSingleton<IPersonRepository, PersonRepository>();
    }
}
