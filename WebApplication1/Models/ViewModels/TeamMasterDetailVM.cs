using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class TeamMasterDetailVM
    {
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Team Name is required")]
        [Display(Name = "Team Name")]
        public string Name { get; set; } = default!;

        [Display(Name = "Upload Logo")]
        public IFormFile? LogoFile { get; set; }

        public string? CurrentLogoUrl { get; set; }

        [Display(Name = "Established Date")]
        [DataType(DataType.Date)]
        public DateTime? EstablishedDate { get; set; }

        [Display(Name = "Annual Revenue ($)")]
        [Range(0, double.MaxValue, ErrorMessage = "Revenue must be positive")]
        public decimal Revenue { get; set; }

        [Display(Name = "Active Team")]
        public bool IsActive { get; set; } = true;

        public List<PlayerEntryVM> Players { get; set; } = new List<PlayerEntryVM>();
    }

    public class PlayerEntryVM
    {
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "Player Name is required")]
        public string FullName { get; set; } = default!;

        public PlayerRole Role { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}