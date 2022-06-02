using System;
using System.ComponentModel.DataAnnotations;

namespace IceCreamClient.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Email { get; set; }
        [StringLength(150, MinimumLength = 3)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Password { get; set; }
        public bool? IsMember { get; set; }
        public DateTime? RegisterDay { get; set; }
        public int? IdMemberOption { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Username { get; set; }
    }
}
