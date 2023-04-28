using SogetiTODO.Repositories.Abstractions;
using SogetiTODO.Repositories.Implementations;

namespace SogetiTODO.Infrastructure.Installers;

public class RepositoriesInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDataSeeder, DataSeeder>();
        services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
    }
}