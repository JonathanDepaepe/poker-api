namespace DataAccessLayer.Models
{
    public partial class TournamentReservation
    {
        public int TournamentId { get; set; }
        public string MemberId { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Tournament Tournament { get; set; } = null!;
    }
}
