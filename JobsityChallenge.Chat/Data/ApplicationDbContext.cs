using JobsityChallenge.Chat.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobsityChallenge.Chat.Data;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ChatroomEntity> Chatrooms { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .Property(e => e.FirstName)
            .HasMaxLength(250);

        modelBuilder.Entity<UserEntity>()
            .Property(e => e.LastName)
            .HasMaxLength(250);

        modelBuilder.Entity<MessageEntity>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<MessageEntity>()
            .Property(e => e.UserId)            
            .IsRequired(false);

        modelBuilder.Entity<MessageEntity>()            
            .HasOne(e => e.User)            
            .WithMany(m => m.Messages)            
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<MessageEntity>()
            .HasOne(e => e.Chatroom)
            .WithMany(m => m.Messages)
            .HasForeignKey(e => e.ChatroomId);

        modelBuilder.Entity<MessageEntity>()
            .Property(e => e.Date)
            .HasDefaultValueSql("getdate()");

        modelBuilder.Entity<ChatroomEntity>()
            .HasMany(c => c.Messages);
    }
}