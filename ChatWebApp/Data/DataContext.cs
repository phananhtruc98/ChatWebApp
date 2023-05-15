using ChatAppAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Runtime.Intrinsics.X86;

namespace ChatAppAPI.Data
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserContact>().HasKey(e => e.Id);
            modelBuilder.Entity<UserContact>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.Contacts)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserContact>()
                .HasOne(uc => uc.Contact)
                .WithMany()
                .HasForeignKey(uc => uc.ContactId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Authenticated User" },
                new Role { Id = 2, Name = "Admin" });

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }

    }
}
