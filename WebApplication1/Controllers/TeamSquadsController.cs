using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeamSquadsController : Controller
    {
        private readonly IGenericRepository<TeamSquad> _squadRepo;
        private readonly IGenericRepository<Tournament> _tournRepo;
        private readonly IGenericRepository<Team> _teamRepo;

        public TeamSquadsController(
            IGenericRepository<TeamSquad> squadRepo,
            IGenericRepository<Tournament> tournRepo,
            IGenericRepository<Team> teamRepo)
        {
            _squadRepo = squadRepo;
            _tournRepo = tournRepo;
            _teamRepo = teamRepo;
        }

        public async Task<IActionResult> Create()
        {
            var model = new TeamSquadCreateVM();
            await PopulateDropdowns(model);
            return PartialView("_CreatePartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamSquadCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var existing = await _squadRepo.FindAsync(s => s.TournamentId == model.TournamentId && s.TeamId == model.TeamId);
                if (existing.Any())
                {
                    ModelState.AddModelError("TeamId", "This team is already registered in this tournament.");
                }
                else
                {
                    var squad = new TeamSquad
                    {
                        TournamentId = model.TournamentId,
                        TeamId = model.TeamId
                    };

                    await _squadRepo.CreateAsync(squad);
                    return Json(new { success = true, message = "Team Registered for Tournament!" });
                }
            }

            await PopulateDropdowns(model);
            return PartialView("_CreatePartial", model);
        }

        private async Task PopulateDropdowns(TeamSquadCreateVM model)
        {
            var tournaments = await _tournRepo.GetAllAsync();
            var teams = await _teamRepo.GetAllAsync();

            model.Tournaments = tournaments.Select(t => new SelectListItem
            {
                Value = t.TournamentId.ToString(),
                Text = t.Name
            });

            model.Teams = teams.Select(t => new SelectListItem
            {
                Value = t.TeamId.ToString(),
                Text = t.Name
            });
        }
    }
}