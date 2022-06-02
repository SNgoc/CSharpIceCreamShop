using System;

namespace IceCreamClient.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
