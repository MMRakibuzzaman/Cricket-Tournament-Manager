using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class TournamentVM
    {
        public int TournamentId { get; set; }

        [Required(ErrorMessage = "Tournament Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public string Status => DateTime.Now < StartDate ? "Upcoming" : 
            (EndDate.HasValue && DateTime.Now > EndDate ? "Completed" : "Live");
    }
}