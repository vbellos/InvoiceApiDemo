using Common.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;


// Dummy data initializer class
public static class DatabaseInitializer
{
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        if (!db.Companies.Any())
        {
            // Generate SoftOne data
            var softOneCompany = new Company
            {
                Id = "softone",
                Name = "SoftOne",
                Users = GenerateUsers("softone", 3)
            };
            db.Companies.Add(softOneCompany);

            db.AccessTokens.Add(new AccessToken
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = softOneCompany.Id,
                IsActive = true,
                Value = "token_softone"
            });

            // Generate edrink data
            var edrink = new Company
            {
                Id = "edrink",
                Name = "edrink",
                Users = GenerateUsers("edrink", 2)
            };
            db.Companies.Add(edrink);

            db.AccessTokens.Add(new AccessToken
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = edrink.Id,
                IsActive = true,
                Value = "token_edrink"
            });

            db.SaveChanges();
        }
    }

    private static IEnumerable<User> GenerateUsers(string companyName, int userCount)
    {
        var random = new Random();
        var users = new List<User>();

        for (int i = 1; i <= userCount; i++)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"User {i}",
                Email = $"user{random.Next(1000, 9999)}@{companyName}.com"
            };
            users.Add(user);
        }

        return users;
    }
    
}



