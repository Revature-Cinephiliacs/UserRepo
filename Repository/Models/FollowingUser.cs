using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    /// <summary>
    /// Model from database-first scaffolding for Repo Layer
    /// </summary>
    public partial class FollowingUser
    {
        public string FollowerUserId { get; set; }
        public string FolloweeUserId { get; set; }

        public virtual User FolloweeUser { get; set; }
        public virtual User FollowerUser { get; set; }
    }
}
