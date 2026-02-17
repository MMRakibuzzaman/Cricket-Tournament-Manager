using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Models.ViewModels;

public class MatchCreateVM
{
    [Required]
    [Display(Name = "Select Tournament")]
    public int TournamentId { get; set; }

    [Required]
    [Display(Name = "Select Venue")]
    public int VenueId { get; set; }

    [Required]
    [Display(Name = "Home Team")]
    public int HomeSquadId { get; set; }

    [Required]
    [Display(Name = "Away Team")]
    public int AwaySquadId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [Display(Name = "Match Date & Time")]
    public DateTime MatchDate { get; set; }

    public IEnumerable<SelectListItem> Tournaments { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Venues { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Squads { get; set; } = new List<SelectListItem>();
}