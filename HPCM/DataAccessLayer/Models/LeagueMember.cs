using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public partial class LeagueMember
    {
        public int LeagueId { get; set; }
        public string MemberId { get; set; }
        public virtual League League { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }


}
