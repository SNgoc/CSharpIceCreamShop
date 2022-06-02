using System.ComponentModel.DataAnnotations;

namespace IceCreamClient.Areas.Admin.Models
{
    public class tbAdmin
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Fullname { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
