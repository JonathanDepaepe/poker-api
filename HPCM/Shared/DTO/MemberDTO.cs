using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class MemberDTO
    {
        public int MemberId { get; set; }
        public string Name { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ProfilePictureUrl { get; set; } = null!;
    }
}
