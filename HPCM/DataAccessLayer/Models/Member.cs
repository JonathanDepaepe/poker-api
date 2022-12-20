using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public partial class Member
    {
        public Member()
        {
            Announcements = new HashSet<Announcement>();
            Clubs = new HashSet<Club>();
        }

        public string MemberId { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; } = null!;
        public string Email { get; set; }
        public MemberTypes MemberAssignedType {get;set;}
        public string ProfilePictureUrl { get; set;} = null!;
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<Club> Clubs { get; set; }
    }

    public enum MemberTypes: Int32
    {
        GhostMember=0,
        BaseMember=1,
        PayingMember=10,
        AdminMember=100
    }

}