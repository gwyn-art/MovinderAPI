using Microsoft.EntityFrameworkCore;
using MovinderAPI.Models;

namespace MovinderAPI.Models
{
    public class MovinderContext : DbContext
    {
        public MovinderContext(DbContextOptions<MovinderContext> options)
            : base(options)
        {
        }

        public DbSet<Invitaiton> Invitaitons { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Invitaiton>().ToTable("Invitaiton");

            modelBuilder.Entity<Invitaiton>()
                .HasOne( i => i.inviter)
                .WithMany( u => u.InvaterPosts)
                .HasForeignKey( i => i.inviterId);
        }
        
    }
}