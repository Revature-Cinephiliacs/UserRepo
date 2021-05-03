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

        public virtual DbSet<FollowingMovie> FollowingMovies { get; set; }
        public virtual DbSet<FollowingUser> FollowingUsers { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<FollowingMovie>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MovieId })
                    .HasName("user_following_movie_pk");

                entity.ToTable("following_movies");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userID");

                entity.Property(e => e.MovieId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("movieID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FollowingMovies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__following__userI__797309D9");
            });

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
                    .HasConstraintName("FK__following__follo__76969D2E");

                entity.HasOne(d => d.FollowerUser)
                    .WithMany(p => p.FollowingUserFollowerUsers)
                    .HasForeignKey(d => d.FollowerUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__following__follo__75A278F5");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notifications");

                entity.Property(e => e.NotificationId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("notificationID");

                entity.Property(e => e.CreatorId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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
                    .HasConstraintName("FK__notificat__userI__0F624AF8");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.ReviewId)
                    .HasColumnName("reviewId")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreationTime)
                    .HasColumnName("creationTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImdbId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("imdbId");

                entity.Property(e => e.Review1)
                    .HasColumnType("text")
                    .HasColumnName("review");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.UsernameId).HasColumnName("usernameId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "UQ__users__AB6E6164D3A26C5C")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "UQ__users__F3DBC572EED267C9")
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
