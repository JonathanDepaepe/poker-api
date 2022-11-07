using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public partial class MemberType
    {
        public MemberType()
        {
            Members = new HashSet<Member>();
        }

        public int TypeId { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<Member> Members { get; set; }
    }
}
