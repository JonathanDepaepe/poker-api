namespace DataAccessLayer.Models
{
    public partial class ClubMember
    {
        public int ClubId { get; set; }
        public string MemberId { get; set; }
        public ClubRoles Role { get; set; }
        public virtual Club Club { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }


    public enum ClubRoles:Int32
    {
        BaseRole=1,
        ModeratorRole=2,
        AdminRole=3,
        SpectatorRole=9
    }
}
