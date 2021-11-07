using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class RoleModel
    {
        [Required]
        [StringLength(50,MinimumLength = 2)]
        public string Name { get; set; }
    }
}