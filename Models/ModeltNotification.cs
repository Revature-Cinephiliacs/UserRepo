using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalModels
{
    /// <summary>
    /// DTO Object for Frontend User
    /// </summary>
    public class ModelNotification
    {
        public string UserId { get; set; }
        public Guid OtherId { get; set; }
        public List<string> Followers { get; set; }
    }
}
