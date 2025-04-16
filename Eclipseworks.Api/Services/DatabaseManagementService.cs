using Eclipseworks.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Eclipseworks.Api.Services;

public class DatabaseManagementService
{
    public static void MigrateDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
        if (dbContext != null)
        {
            dbContext.Database.Migrate();
        }
        else
        {
            throw new InvalidOperationException("AppDbContext could not be resolved from the service provider.");
        }
    }
}
