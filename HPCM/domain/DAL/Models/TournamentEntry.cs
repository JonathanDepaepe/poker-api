using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class TournamentEntry
    {
        public int TournamentId { get; set; }
        public int MemberId { get; set; }
        public int? Position { get; set; }
        public DateTime RegistrationDateTime { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Tournament Tournament { get; set; } = null!;
    }
}
