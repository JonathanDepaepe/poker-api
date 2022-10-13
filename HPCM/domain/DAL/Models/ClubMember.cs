using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ClubMember
    {
        public int ClubId { get; set; }
        public int MemberId { get; set; }
        public string Role { get; set; } = null!;

        public virtual Club Club { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
