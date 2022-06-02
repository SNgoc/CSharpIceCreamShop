using System;
using System.Collections.Generic;

#nullable disable

namespace IceCreamApi.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            Comments = new HashSet<Comment>();
        }

        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Ingredents { get; set; }
        public string Preparation { get; set; }
        public string Thumbnail { get; set; }
        public bool? PayingRequired { get; set; }
        public bool? Status { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public int? CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
