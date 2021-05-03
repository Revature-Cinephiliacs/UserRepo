using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalModels
{
    /// <summary>
    /// DTO Object for Frontend User
    /// </summary>
    public class NotificationDTO
    {
        public string NotificationId { get; set; }
        public Guid OtherId { get; set; }
        public string FromService { get; set; }
        public string CreatorId { get; set; }
        public string CreatorUsername { get; set; }
    }
}
