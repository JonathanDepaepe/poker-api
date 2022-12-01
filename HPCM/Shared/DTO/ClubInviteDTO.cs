using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ClubInviteDTO
    {
        [Required]
        public string creatorId { get; set; }
        [Required]
        public string memberId { get; set; }
        [Required]
        public int clubId { get; set; }
        [Required]
        public string role { get; set; }
        [Required]
        public int duration { get; set; }
    }
}
