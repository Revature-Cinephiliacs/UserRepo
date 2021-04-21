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
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<FollowingMovie>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.MovieId })
                    .HasName("user_following_movie_pk");

                entity.ToTable("following_movies");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.MovieId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("movieID");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.FollowingMovies)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__following__usern__66603565");
            });

            modelBuilder.Entity<FollowingUser>(entity =>
            {
                entity.HasKey(e => new { e.Follower, e.Followee })
                    .HasName("follower_followee_pk");

                entity.ToTable("following_users");

                entity.Property(e => e.Follower)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("follower");

                entity.Property(e => e.Followee)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("followee");

                entity.HasOne(d => d.FolloweeNavigation)
                    .WithMany(p => p.FollowingUserFolloweeNavigations)
                    .HasForeignKey(d => d.Followee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__following__follo__5FB337D6");

                entity.HasOne(d => d.FollowerNavigation)
                    .WithMany(p => p.FollowingUserFollowerNavigations)
                    .HasForeignKey(d => d.Follower)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__following__follo__5EBF139D");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__users__F3DBC5734CF2C035");

                entity.ToTable("users");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("username");

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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
