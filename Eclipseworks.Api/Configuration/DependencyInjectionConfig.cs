using Eclipseworks.Infra.Context;
using Eclipseworks.Service;
using Eclipseworks.Service.Interfaces;

namespace Eclipseworks.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        services.AddScoped<AppDbContext>();

        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IProjectService, ProjectService>();

        services.AddHttpClient();
    }
}
