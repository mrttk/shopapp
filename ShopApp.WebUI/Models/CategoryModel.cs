using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShopApp.Entity;

namespace ShopApp.WebUI.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50,MinimumLength =2)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(50,MinimumLength =2)]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
    }
}