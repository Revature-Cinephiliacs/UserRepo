﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    /// <summary>
    /// Model from database-first for repo
    /// </summary>
    public partial class Review
    {
        public Guid ReviewId { get; set; }
        public Guid UsernameId { get; set; }
        public string ImdbId { get; set; }
        public double Score { get; set; }
        public string Review1 { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
