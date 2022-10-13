using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Club
    {
        public Club()
        {
            Announcements = new HashSet<Announcement>();
            Leagues = new HashSet<League>();
        }

        public int ClubId { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public short Public { get; set; }
        public DateTime CreationDateTime { get; set; }

        public virtual Member Owner { get; set; } = null!;
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<League> Leagues { get; set; }
    }
}
