using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceCreamClient.Models
{
    public class BookIceCream
    {
        public int BookId { get; set; }
        public int? CatId { get; set; }
        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }
        [StringLength(100)]
        [Required]
        public string Author { get; set; }
        public string Description { get; set; }
        [Range(1, 1000)]
        [DataType(DataType.Currency)]
        public double? Price { get; set; }
        public string Image { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public virtual Category Cat { get; set; }
    }
}
