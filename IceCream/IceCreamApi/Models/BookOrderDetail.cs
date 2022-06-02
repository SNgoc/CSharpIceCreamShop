using System;
using System.Collections.Generic;


namespace IceCreamApi.Models
{
    public partial class BookOrderDetail
    {
        public int Id { get; set; }
        public int? BookOrderId { get; set; }
        public int? BookId { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public virtual BookOrder BookOrder { get; set; }
        
    }
}
