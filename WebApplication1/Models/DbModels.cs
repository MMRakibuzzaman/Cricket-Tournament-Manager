using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public enum MatchStatus { Scheduled, Live, Completed, Abandoned }
    public enum WicketType { Bowled, Caught, LBW, RunOut, Stumped }
    public enum PlayerRole { Batsman, Bowler, AllRounder, WicketKeeper }


    public class Tournament
    {
        [Key]
        public int TournamentId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public virtual List<TeamSquad> Squads { get; set; } = new();
        public virtual List<Match> Matches { get; set; } = new();
    }

    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

        [StringLength(500)]
        public string? LogoUrl { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EstablishedDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Revenue { get; set; }

        public bool IsActive { get; set; }

        public virtual List<Player> Players { get; set; } = new();

        public virtual List<TeamSquad> Squads { get; set; } = new();
    }

    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = default!;

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500)]
        public string? PhotoUrl { get; set; }

        public PlayerRole Role { get; set; }

        public int? TeamId { get; set; }
        [ForeignKey("TeamId")]
        public virtual Team? Team { get; set; }
    }

    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

    }


    public class TeamSquad
    {
        [Key]
        public int SquadId { get; set; }

        public int TournamentId { get; set; }
        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament { get; set; } = null!;

        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; } = null!;

        public virtual List<SquadPlayer> Players { get; set; } = new();
    }

    public class SquadPlayer
    {
        [Key]
        public int SquadPlayerId { get; set; }

        public int SquadId { get; set; }
        [ForeignKey("SquadId")]
        public virtual TeamSquad Squad { get; set; } = null!;

        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; } = null!;

        public bool IsCaptain { get; set; }
    }

    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        public int TournamentId { get; set; }
        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament { get; set; } = null!;

        public int VenueId { get; set; }
        [ForeignKey("VenueId")]
        public virtual Venue Venue { get; set; } = null!;

        public int HomeSquadId { get; set; }
        [ForeignKey("HomeSquadId")]
        public virtual TeamSquad? HomeSquad { get; set; } = null!;

        public int AwaySquadId { get; set; }
        [ForeignKey("AwaySquadId")]
        public virtual TeamSquad? AwaySquad { get; set; } = null!;

        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }

        public int? HomeRuns { get; set; }
        public int? HomeWickets { get; set; }

        [Column(TypeName = "decimal(4, 1)")]
        public decimal? HomeOvers { get; set; } 

        public int? AwayRuns { get; set; }
        public int? AwayWickets { get; set; }

        [Column(TypeName = "decimal(4, 1)")]
        public decimal? AwayOvers { get; set; }

        public int? WinnerSquadId { get; set; }
        public string? ResultDescription { get; set; } 
    }
}