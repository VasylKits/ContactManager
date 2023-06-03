using ContactManager.Db;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Extensions.MigrationManager;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost webHost)
    {
        using (var scope = webHost.Services.CreateScope())
        {
            using var appContext = scope.ServiceProvider.GetRequiredService<ContactContext>();
            try
            {
                appContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return webHost;
    }
}