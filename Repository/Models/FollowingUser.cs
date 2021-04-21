using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class FollowingUser
    {
        public string Follower { get; set; }
        public string Followee { get; set; }

        public virtual User FolloweeNavigation { get; set; }
        public virtual User FollowerNavigation { get; set; }
    }
}
