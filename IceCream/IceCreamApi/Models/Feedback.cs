using System;
using System.Collections.Generic;

#nullable disable

namespace IceCreamApi.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
    }
}
