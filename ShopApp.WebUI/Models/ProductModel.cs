using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        [Range(1,100000)]
        public double? Price { get; set; }

        [Required]
        [StringLength(500,MinimumLength =5)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public bool IsApproved { get; set; }

        public List<Entity.Category> SelectedCategories { get; set; }
    }
}