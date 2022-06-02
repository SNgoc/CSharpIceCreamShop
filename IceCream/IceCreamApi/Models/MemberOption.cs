using System;
using System.Collections.Generic;

#nullable disable

namespace IceCreamApi.Models
{
    public partial class MemberOption
    {
        public MemberOption()
        {
            Members = new HashSet<Member>();
        }

        public int Id { get; set; }
        public string MemberOption1 { get; set; }
        public string Day { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
