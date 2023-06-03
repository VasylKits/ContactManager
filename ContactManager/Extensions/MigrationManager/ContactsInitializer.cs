using ContactManager.Db;
using ContactManager.Models;

namespace ContactManager.Extensions.MigrationManager;

public static class ContactsInitializer
{
    public static IHost Seed(this IHost webHost)
    {
        using var scope = webHost.Services.CreateScope();
        {
            using var context = scope.ServiceProvider.GetRequiredService<ContactContext>();

            try
            {
                context.Database.EnsureCreated();

                var contacts = context.Contacts.FirstOrDefault();

                if (contacts is null)
                {
                    context.Contacts.AddRange(
                        new Contact { Name = "Adam", DateOfBirth = new DateTime(1995, 4, 8), Married = true, Phone = "0676739765", Salary = 1000 },
                        new Contact { Name = "Ben", DateOfBirth = new DateTime(1976, 12, 11), Married = false, Phone = "0967865457", Salary = 900 },
                        new Contact { Name = "Nika", DateOfBirth = new DateTime(2002, 8, 5), Married = true, Phone = "0666700324", Salary = 1100 },
                        new Contact { Name = "Eva", DateOfBirth = new DateTime(2000, 4, 1), Married = false, Phone = "0956745680", Salary = 600 },
                        new Contact { Name = "Inna", DateOfBirth = new DateTime(1993, 2, 10), Married = false, Phone = "0670908764", Salary = 750 }
                        );

                    context.SaveChanges();
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return webHost;
    }
}