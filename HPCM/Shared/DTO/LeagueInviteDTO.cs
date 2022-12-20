using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class LeagueInviteDTO
    {
        [Required]
        public string memberId { get; set; }
        [Required]
        public int leagueId { get; set; }
        [Required]
        public int duration { get; set; }
    }
}
