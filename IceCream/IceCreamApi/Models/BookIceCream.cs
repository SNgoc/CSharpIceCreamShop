using System;
using System.Collections.Generic;

#nullable disable

namespace IceCreamApi.Models
{
    public partial class BookIceCream
    {
        public int BookId { get; set; }
        public int? CatId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public string Image { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Category Cat { get; set; }
    }
}
