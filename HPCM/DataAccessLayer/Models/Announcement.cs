using System;

namespace DataAccessLayer.Models
{
    public partial class Announcement
    {
        public int PostId { get; set; }
        public int ClubId { get; set; }
        public string CreatorId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreationDateTime { get; set; }

        public virtual Club Club { get; set; } = null!;
        public virtual Member Creator { get; set; } = null!;
    }
}
