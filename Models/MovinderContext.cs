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

        public DbSet<Invitation> Invitaitons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Respond> Responds { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Invitation>().ToTable("Invitaiton");
            modelBuilder.Entity<Respond>()
                .ToTable("Respond")
                .HasKey(r => new {r.invitationId, r.respondentId});

            modelBuilder.Entity<Invitation>()
                .HasOne( i => i.inviter)
                .WithMany( u => u.InvaterPosts)
                .HasForeignKey( i => i.inviterId);

            modelBuilder.Entity<Respond>()
                .HasOne( r => r.responder)
                .WithMany( u => u.Responds)
                .HasForeignKey( i => i.respondentId);

            modelBuilder.Entity<Respond>()
                .HasOne( r => r.invitaiton)
                .WithMany( i => i.responders)
                .HasForeignKey( i => i.invitationId);
        }
        
    }
}