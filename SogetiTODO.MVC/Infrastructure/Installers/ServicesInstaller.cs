using SogetiTODO.Services.Abstractions;
using SogetiTODO.Services.Implementations;

namespace SogetiTODO.Infrastructure.Installers;

public class ServicesInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITodoService, TodoService>();
    }
}