using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Entities.ManyToMany;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;

namespace signalr_best_practice_dl_postgresql
{
    public class PostgreSqlContext : DbContext, IDbContext
    {
        IDatabaseSettings _databaseSettings;

        public static ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                              category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                              .AddConsole();
        });

        public PostgreSqlContext()
        {
            _databaseSettings = new PostgreSqlSettings();
        }

        public PostgreSqlContext(IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cs = _databaseSettings.GetConnectionString();

            optionsBuilder.UseLazyLoadingProxies()
                          .UseLoggerFactory(MyLoggerFactory) // Warning: Do not create a new ILoggerFactory instance each time
                          .UseNpgsql(cs);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // REFRESHTOKEN
            modelBuilder.Entity<RefreshToken>()
                .HasKey(x => new { x.Id, x.Token });

            // USER_ROLE
            modelBuilder.Entity<UserRole>()
                .HasKey(x => new { x.Id, x.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.Id);

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(x => x.RoleId);
        }


        public ICollection<T> GetDataInstances<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges<T>() where T : class, new()
        {
            SaveChanges();
        }

        ICollection<T> IDbContext.GetDataInstances<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}
