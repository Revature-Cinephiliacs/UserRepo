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
        public string OtherId { get; set; }
        public string Reviewid { set { if(!string.IsNullOrEmpty(value)) OtherId = value; } }
        public string DiscussionId { set { if(!string.IsNullOrEmpty(value)) OtherId = value; } }
        public string CommentId { set { if(!string.IsNullOrEmpty(value)) OtherId = value; } }
        public List<string> Followers { get; set; }
    }
}
