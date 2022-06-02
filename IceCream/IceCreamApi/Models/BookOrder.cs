using System;
using System.Collections.Generic;

#nullable disable

namespace IceCreamApi.Models
{
    public partial class BookOrder
    {
        public BookOrder()
        {
            BookOrderDetails = new HashSet<BookOrderDetail>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fullname { get; set; }
        public int? TotalPrice { get; set; }
        public string Phone { get; set; }

        public bool? IsCompleted { get; set; }

        public virtual ICollection<BookOrderDetail> BookOrderDetails { get; set; }
    }
}
