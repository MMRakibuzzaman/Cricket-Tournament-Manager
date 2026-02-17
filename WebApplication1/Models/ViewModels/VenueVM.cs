using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class VenueVM
    {
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = default!;
    }
}