namespace DataAccessLayer.Models
{
    public partial class TournamentLink
    {
        public int TournamentId { get; set; }
        public int? ClubId { get; set; }
        public int? LeagueId { get; set; }
        public virtual Tournament Tournament { get; set; } = null!;
    }
}
