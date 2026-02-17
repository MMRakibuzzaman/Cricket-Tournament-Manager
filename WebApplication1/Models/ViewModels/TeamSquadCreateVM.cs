using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class TeamSquadCreateVM
    {
        [Required]
        [Display(Name = "Select Tournament")]
        public int TournamentId { get; set; }

        [Required]
        [Display(Name = "Select Team")]
        public int TeamId { get; set; }

        public IEnumerable<SelectListItem> Tournaments { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Teams { get; set; } = new List<SelectListItem>();
    }
}