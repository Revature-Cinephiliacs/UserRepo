using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Repository.Models
{
    public partial class Cinephiliacs_UserContext : DbContext
    {
        public Cinephiliacs_UserContext()
        {
        }

        public Cinephiliacs_UserContext(DbContextOptions<Cinephiliacs_UserContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FollowingUser> FollowingUsers { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<FollowingUser>(entity =>
            {
                entity.HasKey(e => new { e.FollowerUserId, e.FolloweeUserId })
                    .HasName("follower_followee_pk");

                entity.ToTable("following_users");

                entity.Property(e => e.FollowerUserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("follower_userID");

                entity.Property(e => e.FolloweeUserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("followee_userID");

                entity.HasOne(d => d.FolloweeUser)
                    .WithMany(p => p.FollowingUserFolloweeUsers)
                    .HasForeignKey(d => d.FolloweeUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__following__follo__619B8048");

                entity.HasOne(d => d.FollowerUser)
                    .WithMany(p => p.FollowingUserFollowerUsers)
                    .HasForeignKey(d => d.FollowerUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__following__follo__60A75C0F");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notifications");

                entity.Property(e => e.NotificationId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("notificationID");

                entity.Property(e => e.FromService)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OtherId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("otherID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__notificat__userI__6477ECF3");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "UQ__users__AB6E61640949F73B")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "UQ__users__F3DBC5722C5C4BA0")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userID");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Permissions).HasColumnName("permissions");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
