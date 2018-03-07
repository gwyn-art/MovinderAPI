using Microsoft.EntityFrameworkCore;

namespace MovinderAPI.Models
{
    public class InvitationContext : DbContext
    {
        public InvitationContext(DbContextOptions<InvitationContext> options)
            : base(options)
        {
        }

        public DbSet<Invitaiton> Invitaitons { get; set; }

    }
}