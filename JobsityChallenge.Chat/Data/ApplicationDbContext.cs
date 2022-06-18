using JobsityChallenge.Chat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobsityChallenge.Chat.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>()
                .Property(e => e.FirstName)
                .HasMaxLength(250);

            modelBuilder.Entity<UserModel>()
                .Property(e => e.LastName)
                .HasMaxLength(250);

            modelBuilder.Entity<MessageModel>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<MessageModel>()
                .HasOne(e => e.User)
                .WithMany(m => m.Messages)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<MessageModel>()
                .Property(e => e.Date)
                .HasDefaultValueSql("getdate()");
        }
    }
}