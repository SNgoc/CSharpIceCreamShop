using System.ComponentModel.DataAnnotations;

namespace IceCreamClient.Areas.Admin.Models
{
    public class ITbAdmin
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Password { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Password2 { get; set; }
    }
}
