using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Repositories;
using User.Domain.Models;
using Microsoft.EntityFrameworkCore;
using User.Domain.Helpers;

namespace User.Domain.Seeders;

public class UsersSeeder(DataContext context) : IUsersSeeder
{
    public async Task Seed()
    {
        if (!context.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role { Name = "Administrator" },
                new Role { Name = "Employee" },
                new Role { Name = "Client" },
                new Role { Name = "Manager" },
                new Role { Name = "Support" },
                new Role { Name = "Blacklist" }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var role_client = context.Roles.Where(r => r.Name == "Client").ToList();
            var role_admin = context.Roles.Where(r => new[] { "Administrator", "Client", "Employee" }.Contains(r.Name)).ToList();

            var users = new List<User.Domain.Models.User>
            {
                new User.Domain.Models.User { Username = "Seeder_User_A", Roles = role_client, Email = "seeder@email.a", PasswordHash = PasswordHelper.Hash("Seeder_Password_A")},
                new User.Domain.Models.User { Username = "Seeder_User_B", Roles = role_client, Email = "seeder@email.b", PasswordHash = PasswordHelper.Hash("Seeder_Password_B") },
                new User.Domain.Models.User { Username = "Seeder_User_C", Roles = role_client, Email = "seeder@email.c", PasswordHash = PasswordHelper.Hash("Seeder_Password_C") },
                new User.Domain.Models.User { Username = "admin", Roles = role_admin, Email = "Admin@email.a", PasswordHash = PasswordHelper.Hash("password") },
                new User.Domain.Models.User { Username = "admin2", Roles = role_admin, Email = "test.bien.test@gmail.com", PasswordHash = PasswordHelper.Hash("password") },
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
