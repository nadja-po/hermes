using Hermes_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hermes_Services.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
       public DbSet<UsersInGroup> UsersInGroup { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UsersInGroup>().HasKey(s => new { s.GroupId, s.UserId });

            modelBuilder.Entity<UsersInGroup>()
                 .HasOne(ub => ub.User)
                 .WithMany(au => au.UsersInGroup)
                 .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UsersInGroup>()
                .HasOne(ub => ub.Group)
                .WithMany()
                .HasForeignKey(ub => ub.GroupId);

        }
    }
}
