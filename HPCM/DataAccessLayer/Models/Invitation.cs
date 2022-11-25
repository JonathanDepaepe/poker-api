using System;

namespace DataAccessLayer.Models
{
    public partial class Invitation
    {
        public string MemberId { get; set; }
        public int ClubId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string InvitationHash { get; set; } = null!;
        public ClubRoles Role { get; set; }
        public virtual Club Club { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
