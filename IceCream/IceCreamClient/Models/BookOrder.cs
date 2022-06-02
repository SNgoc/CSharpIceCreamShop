using System;
using System.ComponentModel.DataAnnotations;

namespace IceCreamClient.Models
{
    public class BookOrder
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fullname { get; set; }
        public int? TotalPrice { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{1,50}$",
        ErrorMessage = "Please Just Input Number")]
        public string Phone { get; set; }

        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
