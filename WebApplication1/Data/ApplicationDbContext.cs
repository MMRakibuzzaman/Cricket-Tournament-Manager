using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<TeamSquad> TeamSquads { get; set; }
    public DbSet<SquadPlayer> SquadPlayers { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Match>()
            .HasOne(m => m.HomeSquad)
            .WithMany()
            .HasForeignKey(m => m.HomeSquadId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Match>()
            .HasOne(m => m.AwaySquad)
            .WithMany()
            .HasForeignKey(m => m.AwaySquadId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}