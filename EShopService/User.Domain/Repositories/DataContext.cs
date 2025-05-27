using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;


namespace User.Domain.Repositories;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User.Domain.Models.User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User.Domain.Models.User>()
            .Navigation(u => u.Roles)
            .AutoInclude();

        // If you have other configuration, keep it here as well
        modelBuilder.Entity<User.Domain.Models.User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users);
    }

}
