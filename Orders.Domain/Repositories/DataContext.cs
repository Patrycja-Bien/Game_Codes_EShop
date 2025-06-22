using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Domain.Models;

namespace Orders.Domain.Repositories;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Address> Addresses { get; set; }
}
