﻿using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Announcement
    {
        public int PostId { get; set; }
        public int ClubId { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreationDateTime { get; set; }

        public virtual Club Club { get; set; } = null!;
        public virtual Member Creator { get; set; } = null!;
    }
}