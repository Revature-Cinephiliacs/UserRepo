using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class FollowingUser
    {
        /// <summary>
        /// Model from database-first for repo
        /// </summary>
        public string FollowerUserId { get; set; }
        public string FolloweeUserId { get; set; }

        public virtual User FolloweeUser { get; set; }
        public virtual User FollowerUser { get; set; }
    }
}
