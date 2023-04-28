using SogetiTODO.Infrastructure.Installers;

namespace SogetiTODO.Infrastructure.Extensions;

public static class ServiceExtension
{
    public static void InstallServicesFromAssembly(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        services.AddControllersWithViews();
        var installers = typeof(Program).Assembly.ExportedTypes
            .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
        installers.ForEach(installer => installer.InstallServices(services, configuration));
    }
}