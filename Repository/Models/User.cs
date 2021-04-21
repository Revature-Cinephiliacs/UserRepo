using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class User
    {
        public User()
        {
            FollowingMovies = new HashSet<FollowingMovie>();
            FollowingUserFolloweeNavigations = new HashSet<FollowingUser>();
            FollowingUserFollowerNavigations = new HashSet<FollowingUser>();
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte Permissions { get; set; }

        public virtual ICollection<FollowingMovie> FollowingMovies { get; set; }
        public virtual ICollection<FollowingUser> FollowingUserFolloweeNavigations { get; set; }
        public virtual ICollection<FollowingUser> FollowingUserFollowerNavigations { get; set; }

    }
}
