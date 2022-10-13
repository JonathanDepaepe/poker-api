using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Member
    {
        public Member()
        {
            Announcements = new HashSet<Announcement>();
            Clubs = new HashSet<Club>();
        }

        public int MemberId { get; set; }
        public string Name { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public int TypeId { get; set; }
        public string ProfilePictureUrl { get; set; } = null!;

        public virtual MemberType Type { get; set; } = null!;
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<Club> Clubs { get; set; }
    }
}
