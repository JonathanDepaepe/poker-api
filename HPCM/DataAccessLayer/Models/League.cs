namespace DataAccessLayer.Models
{
    public partial class League
    {
        public int LeagueId { get; set; }
        public int ClubId { get; set; }
        public string Name { get; set; } = null!;
        public bool Public { get; set; }
        public string? Description { get; set; }
        public virtual Club Club { get; set; } = null!;
    }
}
