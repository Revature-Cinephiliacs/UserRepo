using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    /// <summary>
    /// Model from database-first for repo
    /// </summary>
    public partial class User
    {
        public User()
        {
            FollowingMovies = new HashSet<FollowingMovie>();
            FollowingUserFolloweeUsers = new HashSet<FollowingUser>();
            FollowingUserFollowerUsers = new HashSet<FollowingUser>();
            Notifications = new HashSet<Notification>();
        }

        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte Permissions { get; set; }

        public virtual ICollection<FollowingMovie> FollowingMovies { get; set; }
        public virtual ICollection<FollowingUser> FollowingUserFolloweeUsers { get; set; }
        public virtual ICollection<FollowingUser> FollowingUserFollowerUsers { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
