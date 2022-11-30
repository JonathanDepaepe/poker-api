using System;

namespace DataAccessLayer.Models
{
    public partial class LeagueInvitation
    {
        public string MemberId { get; set; }
        public int LeagueId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string InvitationHash { get; set; } = null!;
        public virtual League League { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
