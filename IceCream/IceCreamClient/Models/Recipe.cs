using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamClient.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Ingredents { get; set; }
        public string Preparation { get; set; }
        public string Thumbnail { get; set; }
        public bool? PayingRequired { get; set; }
        public bool? Status { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
    }
}
