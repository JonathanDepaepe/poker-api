using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Jwt
    {
        public int MemberId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime TokenExpirationDate { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
