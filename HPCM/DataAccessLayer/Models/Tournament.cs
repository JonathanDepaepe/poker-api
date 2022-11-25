using System;

namespace DataAccessLayer.Models
{
    public partial class Tournament
    {
        public int TournamentId { get; set; }
        public bool Public { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Location { get; set; } = null!;
        public int MaxPlayerCount { get; set; }
    }
}
