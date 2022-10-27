using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class League
    {
        public int LeagueId { get; set; }
        public int ClubId { get; set; }
        public string Name { get; set; } = null!;
        public short Public { get; set; }
        public string? Description { get; set; }
        public virtual Club Club { get; set; } = null!;
    }
}
