using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GlobalModels
{
    /// <summary>
    /// Model sent from other services
    /// </summary>
    public class ModelNotification
    {
        public string Usernameid { get; set; }
        public Guid OtherId { get; set; }
        public Guid Reviewid { set { OtherId = value; } }
        public Guid DiscussionId { set { OtherId = value; } }
        public Guid CommentId { set { OtherId = value; } }
        public List<string> Followers { get; set; }
    }
}
