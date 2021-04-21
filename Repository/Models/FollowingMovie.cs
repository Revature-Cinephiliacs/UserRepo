using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    /// <summary>
    /// Database model for table of user->movie following
    /// </summary>
    public partial class FollowingMovie
    {
        public string UserId { get; set; }
        public string MovieId { get; set; }

        public virtual User User { get; set; }
    }
}
