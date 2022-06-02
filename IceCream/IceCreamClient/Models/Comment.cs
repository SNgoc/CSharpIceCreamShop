using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamClient.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string EmailUser { get; set; }
        public string NameUser { get; set; }
        public string Reply { get; set; }
        public int? RecipeId { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool? isReplied { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
