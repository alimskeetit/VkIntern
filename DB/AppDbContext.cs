using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<UserState> UserStates { get; set; }

    public AppDbContext(DbContextOptions options): base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserState>().HasData(
            new UserState
            {
                Id = 1,
                Code = "Admin",
                Description = "Администратор"
            }
        );

        modelBuilder.Entity<UserGroup>().HasData(
            new UserGroup
            {
                Id = 1,
                Code = "Active",
                Description = "Активный пользователь"
            },
            new UserGroup
            {
                Id = 2,
                Code = "Blocked",
                Description = "Заблокированный пользователь"
            }
        );


        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Login = "admin",
                Password = "admin",
                CreatedDate = DateTime.UtcNow,
                StateId = 1,
                GroupId = 1
            }
        );
    }
}