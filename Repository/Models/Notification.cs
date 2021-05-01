using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    /// <summary>
    /// Model from database-first scaffolding for Repo Layer
    /// </summary>
    public partial class Notification
    {
        public string NotificationId { get; set; }
        public string UserId { get; set; }
        public string OtherId { get; set; }
        public string FromService { get; set; }

        public virtual User User { get; set; }
    }
}
