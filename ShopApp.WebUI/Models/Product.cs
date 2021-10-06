using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(30, MinimumLength =2)]
        public string Name { get; set; }
        [Required]
        [Range(1,35000)]
        public double? Price { get; set; }
        public string Description { get; set; }
        
        [Required]
        public string ImageUrl { get; set; }
        
        public bool IsApproved { get; set; }
        [Required]
        public int? CategoryId { get; set; }
    }
}